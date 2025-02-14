using UnityEngine;
using Crosstales.RTVoice;
using Crosstales.RTVoice.Model;

/// <summary>
/// Simple example to demonstrate the basic usage of RT-Voice.
/// </summary>
public class SimpleRTVoiceExample : MonoBehaviour
{
    public static SimpleRTVoiceExample instance;



   public string Text = "Hello world, I am RT-Voice!";
   public string Culture = "en";
   public bool UseDefaultVoice;
   public bool SpeakWhenReady;
   public AudioSource Audio;
   public bool UseNative;

   private string uid; //Unique id of the speech


    void Awake()
    {
        MakeInstance();
    }

    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
   {
      // Subscribe event listeners
      Speaker.Instance.OnVoicesReady += voicesReady;
      Speaker.Instance.OnSpeakStart += speakStart;
      Speaker.Instance.OnSpeakComplete += speakComplete;
   }

   private void OnDisable()
   {
      if (Speaker.Instance != null)
      {
         // Unsubscribe event listeners
         Speaker.Instance.OnVoicesReady -= voicesReady;
         Speaker.Instance.OnSpeakStart -= speakStart;
         Speaker.Instance.OnSpeakComplete -= speakComplete;
      }
   }


   public void Speak(string text)
   {
      if (UseNative)
      {
         uid = Speaker.Instance.SpeakNative(text, UseDefaultVoice ? null : Speaker.Instance.VoiceForCulture(Culture)); //Speak (native TTS) with the first voice matching the given culture or the default voice
      }
      else
      {
         uid = Speaker.Instance.Speak(text, Audio, UseDefaultVoice ? null : Speaker.Instance.VoiceForCulture(Culture)); //Speak (audio file) with the first voice matching the given culture or the default voice
      }
   }

   private void voicesReady()
   {
      Debug.Log($"RT-Voice: {Speaker.Instance.Voices.Count} voices are ready to use!");

      if (SpeakWhenReady) //Speak after the voices are ready
         Speak("hii");
   }

   private void speakStart(Wrapper wrapper)
   {
      if (wrapper.Uid == uid) //Only write the log message if it's "our" speech
         Debug.Log($"RT-Voice: speak started: {wrapper}");
   }

   private void speakComplete(Wrapper wrapper)
   {
      if (wrapper.Uid == uid) //Only write the log message if it's "our" speech
         Debug.Log($"RT-Voice: speak completed: {wrapper}");
   }
}
// © 2022-2024 crosstales LLC (https://www.crosstales.com)