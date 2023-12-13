using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Diagnostics;


public class CamRenderer : MonoBehaviour
{
    public RawImage image;
    private Texture2D texture;
    private JJCameraManager cameraManager;
    private FrameListener frameListener;

    private Color32[] camData;
    private byte[] camBytes;
    private GCHandle handle;
    private bool frameUpdated;
    private int width = 1280;
    private int height = 720;
    private Stopwatch stopwatch;
    private int frameCount = 0;
    private bool startfirst = true;




    // Start is called before the first frame update
    void Start()
    {
        // texture = new Texture2D(width, height, TextureFormat.RGBA32, false, false);
        // image.texture = texture;
        // cameraManager = new JJCameraManager();
        // // create a NativeFrameListener
        // //frameListener = new FrameListener(onIncomingFrame);
        // // create a FrameListener
        // frameListener = new FrameListener(onIncomingBytes);
        // frameUpdated = false;

        // cameraManager.SetResolutionIndex((int)JJCameraManager.Resoltion.RES_1280x720);

        // cameraManager.SetCameraFrameListener(frameListener);
        // cameraManager.StartCamera((int)JJCameraManager.ColorFormat.COLOR_FORMAT_RGBA);
        // stopwatch = new Stopwatch();
    }

    // Update is called once per frame
    void Update()
    {
        if (Ganzin2DScreenPosCtrl.isEyetrackerActive && startfirst)
        {
            startfirst = false;
            // 這裡啟動需要權限的操作
            texture = new Texture2D(width, height, TextureFormat.RGBA32, false, false);
            image.texture = texture;
            cameraManager = new JJCameraManager();
            // create a NativeFrameListener
            //frameListener = new FrameListener(onIncomingFrame);
            // create a FrameListener
            frameListener = new FrameListener(onIncomingBytes);
            frameUpdated = false;

            cameraManager.SetResolutionIndex((int)JJCameraManager.Resoltion.RES_1280x720);

            cameraManager.SetCameraFrameListener(frameListener);
            cameraManager.StartCamera((int)JJCameraManager.ColorFormat.COLOR_FORMAT_RGBA);
            stopwatch = new Stopwatch();

        }
        else
        {
            return;
        }


        if (frameUpdated)
        {
            if (frameListener.IsNativeListener)
                texture.SetPixels32(camData);
            else
            // NOTE: The length of camBytes is not the same with the
            //       size of the actual pixel bytes. It appears that the first 4
            //       bytes and the last 3 bytes in camBytes are all dummy ones. So
            //       we need to use remaining() to get the actual size then set the
            //       start offset to 4 to fetch the pixel bytes in Unity.
                texture.SetPixelData(camBytes, 0, 4);
            texture.Apply();
            image.texture = texture;
            frameUpdated = false;
        }

        // CameraParameter cp =  cameraManager.GetCameraParmeter();
        // cp.SetContrast(100);
        // cameraManager.SetCameraParameter(cp.javaObject);
    }

    private void onIncomingFrame(in Color32[] data, int width, int height, int format)
    {
        camData = data;
        frameUpdated = true;
        // calculate FPS
        if (stopwatch.ElapsedMilliseconds >= 1000)
        {
            stopwatch.Stop();
            UnityEngine.Debug.Log("FPS: " + frameCount*1000/stopwatch.ElapsedMilliseconds);
            frameCount = 0;
            stopwatch.Restart();
        }
        else if (frameCount == 0)
            stopwatch.Start();
        frameCount+=1;
    }

    private void onIncomingBytes(in byte[] bytes, int width, int height, int format)
    {
        camBytes = bytes;
        frameUpdated = true;
        // calculate FPS
        if (stopwatch.ElapsedMilliseconds >= 1000)
        {
            stopwatch.Stop();
            UnityEngine.Debug.Log("FPS: " + frameCount*1000/stopwatch.ElapsedMilliseconds);
            frameCount = 0;
            stopwatch.Restart();
        }
        else if (frameCount == 0)
            stopwatch.Start();
        frameCount+=1;
    }
}
