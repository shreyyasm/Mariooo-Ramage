using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CutScene : MonoBehaviour
{
    public Animator cameraAnim;
    public Animator BlackAnim;
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.delayedCall(1f, () => { StartCameraAnim(); });
        LeanTween.delayedCall(24f, () => { BlackIn(); });
        LeanTween.delayedCall(27f, () => { SceneManager.LoadScene(1); });
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
