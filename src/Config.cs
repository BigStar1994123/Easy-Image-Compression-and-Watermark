namespace ImageCompression
{
    public class Config
    {
        public MyImage Image { get; set; }
        public WaterMark WaterMark { get; set; }

        public override string ToString()
        {
            return $"Config: Image: {Image}, WaterMark: {WaterMark}";
        }
    }

    public class MyImage
    {
        public Compression Compression { get; set; }
        public Resize Resize { get; set; }

        public override string ToString()
        {
            return $"[Compression: {Compression}, Resize: {Resize}]";
        }
    }

    public class Compression
    {
        public bool Enabled { get; set; }
        public int Level { get; set; }

        public override string ToString()
        {
            return $"[Enabled: {Enabled}, Level: {Level}]";
        }
    }

    public class Resize
    {
        public bool Enabled { get; set; }
        public double Percentage { get; set; }

        public override string ToString()
        {
            return $"[Enabled: {Enabled}, Percentage: {Percentage}]";
        }
    }

    public class WaterMark
    {
        public string Name { get; set; }
        public MyFont Font { get; set; }
        public MyColor Color { get; set; }

        public override string ToString()
        {
            return $"[Font: {Font}, Color: {Color}]";
        }
    }

    public class MyFont
    {
        public double HeightSizePercentage { get; set; }
        public double HeightPositionPercentage { get; set; }
        public double WidthPositionPercentage { get; set; }
    }

    public class MyColor
    {
        public int A { get; set; }
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

        public override string ToString()
        {
            return $"[A: {A}, R: {R}, G: {G}, B: {B}]";
        }
    }
}