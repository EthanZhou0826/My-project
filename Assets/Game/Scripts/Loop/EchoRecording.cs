using System.Collections.Generic;

[System.Serializable]
public class EchoRecording
{
    public List<EchoFrame> frames = new List<EchoFrame>();
    public List<float> shotTimes = new List<float>();

    public EchoRecording Clone()
    {
        EchoRecording copy = new EchoRecording();

        for (int i = 0; i < frames.Count; i++)
        {
            EchoFrame f = new EchoFrame();
            f.time = frames[i].time;
            f.position = frames[i].position;
            f.visualRotationZ = frames[i].visualRotationZ;
            copy.frames.Add(f);
        }

        for (int i = 0; i < shotTimes.Count; i++)
        {
            copy.shotTimes.Add(shotTimes[i]);
        }

        return copy;
    }
}