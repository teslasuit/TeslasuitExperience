using UnityEngine;

public class MusicPlate : MonoBehaviour {


    [SerializeField] private AudioClip clip;
    public AudioClip Clip {
        get { return clip; }
    }

    [SerializeField] private GameObject plate;
    public GameObject GrammPlate {
        get { return plate; }
    }

    public string name;
    public Sprite cover;
}
