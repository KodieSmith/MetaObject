// Copyright (c) 2018 Kodie Smith

using System.IO;

namespace MetaObjects
{
    public class CNullObject: IMetaObject
    {
        public void Read(Stream stream)
        {

        }

        public void Write(Stream stream)
        {

        }
    }

    public class CByteObject: IMetaObject
    {
        public byte One;

        public void Read(Stream stream)
        {
            stream.WriteByte(One);
        }

        public void Write(Stream stream)
        {
            One = (byte)stream.ReadByte();
        }
    }

    public interface IMetaObject
    {
        void Read(Stream stream);

        void Write(Stream stream);
    }

    public class CMetaObject: IMetaObject
    {
        public IMetaObject[] Subordinates;

        public CMetaObject(params IMetaObject[] subordinates)
        {
            Subordinates = new IMetaObject[subordinates.Length];

            subordinates.CopyTo(Subordinates, 0);
        }

        public void Read(Stream stream)
        {
            int i = stream.ReadByte();

            Subordinates[i].Read(stream);

            while ((i = stream.ReadByte()) > 0)
            {
                Subordinates[i].Read(stream);
            }
        }

        public void Write(Stream stream)
        {
            for (int i = 0; i < Subordinates.Length; i++)
            {
                stream.WriteByte((byte)i);

                Subordinates[i].Write(stream);
            }

            stream.WriteByte(byte.MinValue);
        }
    }
}
