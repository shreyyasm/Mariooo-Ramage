using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CutScene : MonoBehaviour
{
    public Animator cameraAnim;
    public Animator BlackAnim;
    public float timer = 24;
    public float timerEnd = 27;
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.delayedCall(1f, () => { StartCameraAnim(); });
        LeanTween.delayedCall(timer, () => { BlackIn(); });
        LeanTween.delayedCall(timerEnd, () => { SceneManager.LoadScene(1); });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartCameraAnim()
    {
        cameraAnim.SetBool("Play", true);
    }
    public void BlackIn()
    {
        BlackAnim.SetBool("Play", true);
    }
}
