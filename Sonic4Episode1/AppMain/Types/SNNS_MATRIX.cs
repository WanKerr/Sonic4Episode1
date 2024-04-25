using Microsoft.Xna.Framework;

public struct SNNS_MATRIX
{
    public float M00;
    public float M01;
    public float M02;
    public float M03;
    public float M10;
    public float M11;
    public float M12;
    public float M13;
    public float M20;
    public float M21;
    public float M22;
    public float M23;
    public float M30;
    public float M31;
    public float M32;
    public float M33;

    public void Assign(NNS_MATRIX matrix)
    {
        this.M00 = matrix.M00;
        this.M01 = matrix.M01;
        this.M02 = matrix.M02;
        this.M03 = matrix.M03;
        this.M10 = matrix.M10;
        this.M11 = matrix.M11;
        this.M12 = matrix.M12;
        this.M13 = matrix.M13;
        this.M20 = matrix.M20;
        this.M21 = matrix.M21;
        this.M22 = matrix.M22;
        this.M23 = matrix.M23;
        this.M30 = matrix.M30;
        this.M31 = matrix.M31;
        this.M32 = matrix.M32;
        this.M33 = matrix.M33;
    }

    public void Assign(ref Matrix matrix)
    {
        this.M00 = matrix.M11;
        this.M01 = matrix.M12;
        this.M02 = matrix.M13;
        this.M03 = matrix.M14;
        this.M10 = matrix.M21;
        this.M11 = matrix.M22;
        this.M12 = matrix.M23;
        this.M13 = matrix.M24;
        this.M20 = matrix.M31;
        this.M21 = matrix.M32;
        this.M22 = matrix.M33;
        this.M23 = matrix.M34;
        this.M30 = matrix.M41;
        this.M31 = matrix.M42;
        this.M32 = matrix.M43;
        this.M33 = matrix.M44;
    }

    public void Assign(ref SNNS_MATRIX matrix)
    {
        this.M00 = matrix.M00;
        this.M01 = matrix.M01;
        this.M02 = matrix.M02;
        this.M03 = matrix.M03;
        this.M10 = matrix.M10;
        this.M11 = matrix.M11;
        this.M12 = matrix.M12;
        this.M13 = matrix.M13;
        this.M20 = matrix.M20;
        this.M21 = matrix.M21;
        this.M22 = matrix.M22;
        this.M23 = matrix.M23;
        this.M30 = matrix.M30;
        this.M31 = matrix.M31;
        this.M32 = matrix.M32;
        this.M33 = matrix.M33;
    }
}
