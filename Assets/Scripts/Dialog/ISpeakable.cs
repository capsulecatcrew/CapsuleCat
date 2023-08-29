namespace Dialog.Scripts
{
    public interface ISpeakable
    {
        public void StartDialog();
        public void FinishCurrSentence();
        public void EndDialog();
    }
}
