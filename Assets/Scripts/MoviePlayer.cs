// Examples of VideoPlayer function

using UnityEngine;
using UnityEngine.Events;

public class MoviePlayer : MonoBehaviour
{

    [SerializeField]
    bool playOnAwake = true,
         isLooping = true;

    [SerializeField] int frameSkip = 0;

    [SerializeField] float targetCameraAlpha = 0.5f;

    [SerializeField] UnityEvent[] eventsAfterReachEnd;

    [SerializeField] UnityEngine.Video.VideoClip video;

    private UnityEngine.Video.VideoPlayer videoPlayer;

    void Start()
    {
        // Will attach a VideoPlayer to the main camera.
        GameObject camera = Camera.main.gameObject; // GameObject.Find("Main Camera");

        // VideoPlayer automatically targets the camera backplane when it is added
        // to a camera object, no need to change videoPlayer.targetCamera.
        videoPlayer = camera.AddComponent<UnityEngine.Video.VideoPlayer>();

        // Play on awake defaults to true. Set it to false to avoid the url set
        // below to auto-start playback since we're in Start().
        videoPlayer.playOnAwake = playOnAwake;

        // By default, VideoPlayers added to a camera will use the far plane.
        // Let's target the near plane instead.
        videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;

        // This will cause our Scene to be visible through the video being played.
        videoPlayer.targetCameraAlpha = targetCameraAlpha;

        // Set the video to play. URL supports local absolute or relative paths.
        // Here, using absolute.
        // videoPlayer.url = "/Users/graham/movie.mov";
        videoPlayer.clip = video;

        // Skip the first 100 frames.
        videoPlayer.frame = frameSkip;

        // Restart from beginning when done.
        videoPlayer.isLooping = isLooping;

        // Each time we reach the end, we slow down the playback by a factor of 10.
        // videoPlayer.loopPointReached += eventsAfterReachEnd;

        // Start playback. This means the VideoPlayer may have to prepare (reserve
        // resources, pre-load a few frames, etc.). To better control the delays
        // associated with this preparation one can use videoPlayer.Prepare() along with
        // its prepareCompleted event.
        videoPlayer.Play();
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        vp.playbackSpeed = vp.playbackSpeed / 10.0F;
    }

    public void HideVideo()
    {
        videoPlayer.targetCameraAlpha = 0f;
    }

    public void ShowVideo()
    {
        videoPlayer.targetCameraAlpha = targetCameraAlpha;
    }

}