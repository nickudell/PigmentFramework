using System;
using System.Collections.Generic;
using System.Speech.Recognition;

namespace Pigment.Engine.Sound
{
    public class Listening : IDisposable
    {
        public enum ListenerState
        {
            Default,
            YesNo,
            Diction
        }

        private SpeechRecognitionEngine ear;
        private ListenerState state;
        private Dictionary<ListenerState, Grammar> grammars;

        public ListenerState State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
                ear.UnloadAllGrammars();
                ear.LoadGrammarAsync(grammars[value]);
            }
        }

        public void ChangeState(ListenerState state)
        {
        }

        public Listening(SpeechRecognized recDelegate)
        {
            ear = new SpeechRecognitionEngine(System.Globalization.CultureInfo.CurrentCulture);
            createGrammars();
            State = ListenerState.Default;
            ear.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recDelegate);
            ear.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(ear_SpeechDetected);
            ear.SetInputToDefaultAudioDevice();
            ear.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void createGrammars()
        {
            grammars = new Dictionary<ListenerState, Grammar>();
            grammars.Add(ListenerState.Default, CreatePositionGrammar());
            grammars.Add(ListenerState.YesNo, CreateYesNoGrammar());
            grammars.Add(ListenerState.Diction, new DictationGrammar());
        }

        private void ear_SpeechDetected(object sender, SpeechDetectedEventArgs e)
        {
        }

        public delegate void SpeechRecognized(object sender, SpeechRecognizedEventArgs e);

        private Grammar CreateYesNoGrammar()
        {
            Choices yesno = new Choices(new string[] { "yes", "no", "ok", "cancel", "all right", "yeah", "why not", "nah", "nope", "not really" });
            GrammarBuilder gram = new GrammarBuilder(yesno);
            gram.Culture = System.Globalization.CultureInfo.CurrentCulture;
            Grammar result = new Grammar(gram);
            result.Name = "YesNo";
            return result;
        }

        private Grammar CreatePositionGrammar()
        {
            Choices posChoice = new Choices(new string[] { "position", "orientation", "angle" });
            GrammarBuilder posGram = new GrammarBuilder(posChoice);

            GrammarBuilder getPhrase1 = new GrammarBuilder("What is my");
            getPhrase1.Append(posGram);
            GrammarBuilder getPhrase2 = new GrammarBuilder("Tell me my");
            getPhrase2.Append(posGram);
            Choices both = new Choices();
            both.Add(getPhrase1);
            both.Add(getPhrase2);
            GrammarBuilder bothgb = both.ToGrammarBuilder();
            bothgb.Culture = System.Globalization.CultureInfo.CurrentCulture;
            Grammar grammar = new Grammar(bothgb);
            grammar.Name = "RequestPhrase";
            return grammar;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="managed"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool managed)
        {
            if (managed)
            {
                //Check if ear still exists and if so, dispose it and set it to null.
                if(ear != null)
                {
                    ear.Dispose();
                    ear = null;
                }
            }
        }
            
    }
}