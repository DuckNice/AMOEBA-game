using UnityEngine;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using NMoodyMaskSystem;

public class StoryRecognizer : MonoBehaviour
{
    public ComputeShader _historyRecognizer;
    static int _handle;

    static StoryRecognizer instance;

    void Awake()
    {
        instance = this;
    }

    public static void SetupComputeShader()
    {
        if (instance == null)
            return;     

        _handle = instance._historyRecognizer.FindKernel("Recognizing");

        //Format all the structures as a 2D Texture.
        int structCount = StructureLibrary.StoryStructures.Count;
        //CHANGE: CeiltoInt on a divide because we pack it in float 4 (previously some pixels would not be set.).
        Texture2D tex = new Texture2D(structCount, Mathf.CeilToInt(StructureLibrary.MaxLength / 4f), TextureFormat.ARGB32, false, true);
        List<uint> lengths = new List<uint>();

        //Not the most optimized algorithm for adding dynamic-length arrays to a texture (float4 4 had to be used, both because automatic formatting to OpenGL is incompatible with anything else, and because textures can only have pixels set in this format).
        for(int i = 0; i < structCount; i++)
        {
            for(int q = 0; q < StructureLibrary.MaxLength; q = q + 4)
            {
                uint structLength = (uint)StructureLibrary.StoryStructures[i].Count;
                lengths.Add(structLength);
                //The for-loops only return false once when they exit, therefore this if-statement is virtually non-existent in cpu-performance while the structure exists.
                //The conditions were reversed, creating an argument out of bounds exception.
                if (structLength > q + 3)
                {
                    tex.SetPixel(i, q, new Color(StructureLibrary.StoryStructures[i][q].ClimacticEffect,
                        StructureLibrary.StoryStructures[i][q+1].ClimacticEffect,
                        StructureLibrary.StoryStructures[i][q+2].ClimacticEffect,
                        StructureLibrary.StoryStructures[i][q+3].ClimacticEffect));
                }
                //If some pixels are left: Set them and fill the rest.
                else if(structLength > q)
                {
                    int remainers = StructureLibrary.StoryStructures[i].Count - q;
                    tex.SetPixel(i, q, new Color((remainers > 0) ? StructureLibrary.StoryStructures[i][q].ClimacticEffect : 0, 
                        (remainers > 1) ? StructureLibrary.StoryStructures[i][q+1].ClimacticEffect : 0, 
                        (remainers > 2) ? StructureLibrary.StoryStructures[i][q+2].ClimacticEffect : 0, 
                        0)
                        );
                }
                //Fill the remaining pixels
                else
                {
                    tex.SetPixel(i,q, new Color(0,0,0,0));
                }
            }
        }

        //CHANGE: Made buffer for lengths.
        ComputeBuffer lengthsBuffer = new ComputeBuffer(lengths.Count, sizeof(uint));
        lengthsBuffer.SetData(lengths.ToArray());
        //CHANGE: Sent the length of hist.
        instance._historyRecognizer.SetBuffer(_handle, "StoryLengths", lengthsBuffer);

        instance._historyRecognizer.SetTexture(_handle, "StoryStructures", tex);
    }


    //As history items are converted they are kept since they will never be altered (either internally, or their order). New historyItems are appended.
    static List<Vector2> _histItems = new List<Vector2>();
    static float _lastTime = 0;


    //CHANGE: Unity limitations have forced the compute-buffer to be executed in the main thread the history-book is updated here. 
    public static void SetHistoryItems()
    {
        //Append new items to the histItems.
        List<HistoryItem> histBook = GameManager.MoodyMask.HistoryBook;
        float newLastTime = _lastTime;

        for (int i = histBook.Count - 1; i >= 0; i--)
        {
            if (histBook[i].GetTime() < _lastTime)
            {
                _histItems.Add(new Vector2(EventLibrary.EventTypes[histBook[i].GetAction().Name], histBook[i].GetTime()));

                newLastTime = (newLastTime > histBook[i].GetTime()) ? newLastTime : histBook[i].GetTime();
            }
            else
            {
                break;
            }
        }

        _lastTime = newLastTime;
    }


    public const string FinishedComputeMutexName = "finishedCompute";
    //CHANGE: Volatile to multithread.
    public static volatile int Closeststructure = 0;
    public static volatile bool ShouldStartCompute  = false;


    public static void PredictClosestStructure(List<Person> peopleToAccountFor)
    {
        if (ShouldStartCompute)
        {
            if (instance == null)
                return;

            //Stop the Predictor thread through a Mutex until we are done recognizing.
            using (Mutex m = new Mutex(true, FinishedComputeMutexName))
            {
                try
                {
                    ShouldStartCompute = false;

                    //Write to the hist-items buffer.
                    ComputeBuffer histBuffer = new ComputeBuffer(_histItems.Count, sizeof(float) * 2);
                    histBuffer.SetData(_histItems.ToArray());
                    //CHANGE: Sent the length of hist.
                    instance._historyRecognizer.SetFloat("HistoryItemsLength", _histItems.Count);
                    instance._historyRecognizer.SetBuffer(_handle, "HistoryItems", histBuffer);
                    histBuffer.Dispose();


                    //Create output buffer which the shader writes to.
                    ComputeBuffer structureFitnessesBuffer = new ComputeBuffer(StructureLibrary.StoryStructures.Count, sizeof(float));
                    instance._historyRecognizer.SetBuffer(_handle, "ReturnStoryFitnesses", structureFitnessesBuffer);

                    //Launch the shader using one thread-group (This will launch 1024 threads, which will each take care of 1 storyStructure [Had I had the time I would have reversed this, and made the thread-groups variable so no excess threads are initialized]). 
                    instance._historyRecognizer.Dispatch(_handle, 1, 1, 1);

                    //Read the structured buffer and place it in an array.
                    float[] structureFitnesses = new float[0];
                    structureFitnessesBuffer.GetData(structureFitnesses);
                    structureFitnessesBuffer.Dispose();

                    //Get the closest fit and return the index in the structures array.
                    int closeststructure = 0;
                    float currentFitness = 0;

                    for (int i = structureFitnesses.Length - 1; i >= 0; i--)
                    {
                        if (currentFitness < structureFitnesses[i])
                        {
                            currentFitness = structureFitnesses[i];
                            closeststructure = i;
                        }
                    }

                    Closeststructure = closeststructure;
                    //CHANGE: Relese the mutex indicating that we are computing.
                    
                }
                finally
                {
                    m.ReleaseMutex();
                }
            }
        }

    }
}