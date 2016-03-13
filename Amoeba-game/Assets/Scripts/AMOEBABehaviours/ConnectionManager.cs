using UnityEngine;

public class ConnectionManager : MonoBehaviour {
    protected GameObject _otherEmotion;
    protected EmotionGenerator _otherGenerator;
    protected EmotionGenerator _owner;
    protected LineRenderer line;


    public static ConnectionManager CreateComponent(GameObject where, GameObject otherEmotion, EmotionGenerator owner, EmotionGenerator otherGenerator)
    {
        ConnectionManager myC = where.AddComponent<ConnectionManager>();
        myC._otherEmotion = otherEmotion;
        myC._otherGenerator = otherGenerator;
        myC._owner = owner;
        
        myC.CreateLineRenderer();

        return myC;
    }


    void CreateLineRenderer()
    {
        line = gameObject.AddComponent<LineRenderer>();
        line.SetColors(Color.black, Color.black);
        line.SetWidth(0.1f, 0.1f);
        line.material = GameManager.Instance.ConnectionMaterial;
    }


    void Update()
    {
        DrawLine();

        if((_owner.transform.position - _otherGenerator.transform.position).magnitude > _owner.ConnectionDistance)
        {
            if(_owner.TerminateConnection(_otherGenerator))
            {
                Destroy(_otherEmotion);
                Destroy(gameObject);
            }
        }
    }


    void DrawLine()
    {
        Vector3[] poss = new Vector3[2];
        poss[0] = transform.position;
        poss[1] = _otherEmotion.transform.position;

        line.SetPositions(poss);
    }
}