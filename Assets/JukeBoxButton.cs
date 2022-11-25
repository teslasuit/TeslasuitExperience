using UnityEngine;
using UnityEngine.Events;
using VRTK;

public class JukeBoxButton : MonoBehaviour {
    [SerializeField] private GramophoneController controller;
    [SerializeField] private int kSong;
    [SerializeField] private Material materialPushed;
    [SerializeField] private Material materialDefault;

    private MeshRenderer _renderer;
    private VRTK_InteractableObject interact;
    private TweenPosition tween;
    private void Awake() {
        interact = GetComponent<VRTK_InteractableObject>();
        tween = GetComponent<TweenPosition>();
        _renderer = GetComponent<MeshRenderer>();

        currentPos = false;
        interact.InteractableObjectTouched += Interact_InteractableObjectTouched;
        _renderer.material = materialDefault;
    }

    private void Interact_InteractableObjectTouched(object sender, InteractableObjectEventArgs e) {
        if (!currentPos) {
            
            controller.StartPlay(kSong);
            currentPos = true;
            onPush.Invoke();
        }
        
    }

    public void SetPushed(int kNumber) {
        if (kNumber == kSong) {
            tween.ResetToBeginning();
            currentPos = true;
            _renderer.material = materialPushed;
        }
    }



    [SerializeField] private UnityEvent onPush;
    private bool currentPos;

    public void ReturnToDefaultPos() {
        if (currentPos) {
            tween.PlayReverse();
            currentPos = false;
            _renderer.material = materialDefault;
        }
    }
}
