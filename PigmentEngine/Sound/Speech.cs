using System;
using System.Collections.Generic;
using System.Speech.Synthesis;

namespace Pigment.Engine.Sound
{
    /*public delegate void Communication(object param);

    public class Speech
    {
        private SpeechSynthesizer vox;

        private const string GREETING = "Hello, sir.";
        private const string ENDING = "Is there anything else I can do for you right now?";

        public bool Speaking { get; private set; }

        public bool EndingSentence { get; private set; }

        private Queue<Pair<string, Pair<Communication, object>>> promptQueue;
        private Pair<Communication, object> currentCallBack;
        private int speed = -1;

        /// <summary>
        /// Speech speed between -10 and 10
        /// </summary>
        public int Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = Math.Min(Math.Max(value, -10), 10);
                vox.Rate = speed;
            }
        }

        public void ChangeSpeed(object newSpeed)
        {
            Speed = (int)newSpeed;
        }

        public Speech()
        {
            vox = new SpeechSynthesizer();
            vox.SelectVoice(vox.GetInstalledVoices()[0].VoiceInfo.Name);
            vox.Rate = speed;
            Speaking = false;
            promptQueue = new Queue<Pair<string, Pair<Communication, object>>>();
            //clear throat
            PromptBuilder pb = new PromptBuilder();
            vox.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(vox_SpeakCompleted);
            if (Speaking)
            {
                promptQueue.Enqueue(new Pair<string, Pair<Communication, object>>("", new Pair<Communication, object>()));
            }
        }

        /// <summary>
        /// speech completed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void vox_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            if (currentCallBack.Item1 != null)
            {
                currentCallBack.Item1.Invoke(currentCallBack.Item2);
            }
            if (promptQueue.Count > 0)
            {
                if (EndingSentence)
                {
                    EndingSentence = false;
                }
                Pair<string, Pair<Communication, object>> prompt = promptQueue.Dequeue();
                currentCallBack = prompt.Item2;
                vox.SpeakAsync(prompt.Item1);
            }
            else
            {
                if (EndingSentence)
                {
                    Speaking = false;
                    EndingSentence = false;
                }
                else
                {
                    EndingSentence = true;
                    vox.SpeakAsync(ENDING);
                }
            }
        }

        /// <summary>
        /// Chance voice and speech rate
        /// </summary>
        /// <param name="rate">new speech rate</param>
        /// <param name="voiceID">new voice</param>
        public void ChangeVoice(int rate, int voiceID)
        {
            vox.Rate = rate;
            vox.SelectVoice(vox.GetInstalledVoices()[voiceID].VoiceInfo.Name);
            //clear throat
            if (Speaking)
            {
                promptQueue.Enqueue(new Pair<string, Pair<Communication, object>>("", new Pair<Communication, object>()));
            }
        }

        /// <summary>
        /// Speaks the input string as soon as it can
        /// </summary>
        /// <param name="text">The string to speak</param>
        public void Speak(string text)
        {
            if (!Speaking)
            {
                Speaking = true;
                vox.SpeakAsync(GREETING);
            }
            promptQueue.Enqueue(new Pair<string, Pair<Communication, object>>(text, new Pair<Communication, object>()));
        }

        public void Speak(string text, Communication callback, object parameter)
        {
            if (!Speaking)
            {
                Speaking = true;
                vox.SpeakAsync(GREETING);
            }
            promptQueue.Enqueue(new Pair<string, Pair<Communication, object>>(text, new Pair<Communication, object>(callback, parameter)));
        }
    }*/
}