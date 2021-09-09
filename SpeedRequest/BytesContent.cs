namespace SpeedRequest
{
    class BytesContent
    {
        public byte[] Content;
        public int Offset;
        public int Count;
        public BytesContent(byte[] content, int offset, int count)
        {
            Content = content;
            Offset = offset;
            Count = count;
        }
    }
}
