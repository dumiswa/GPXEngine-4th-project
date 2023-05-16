using System;
using GXPEngine;	// For Mathf

public struct Vec2 
{
	public float x;
	public float y;

	public Vec2 (float pX = 0, float pY = 0) 
	{
		x = pX;
		y = pY;
	}

	public override string ToString () 
	{
		return String.Format ("({0},{1})", x, y);
	}

	public void SetXY(float pX, float pY) 
	{
		x = pX;
		y = pY;
	}

	public float Length() {
		return Mathf.Sqrt (x * x + y * y);
	}

	public void Normalize() {
		float len = Length ();
		if (len > 0) {
			x /= len;
			y /= len;
		}
	}

	public Vec2 Normalized() {
		Vec2 result = new Vec2 (x, y);
		result.Normalize ();
		return result;
	}


	public float Dot(Vec2 other) {
		float dotProduct = x * other.x + y * other.y;
		return dotProduct;
	}

	public float CrossProduct(Vec2 other) {
		return x * other.y - y * other.x;
	}

	public Vec2 RotateAroundPoint(Vec2 pivot, float angle)
	{
		float sin = Mathf.Sin(angle * (Mathf.PI/180));
		float cos = Mathf.Cos(angle * (Mathf.PI / 180));
		Vec2 rotated = new Vec2(cos * (x - pivot.x) - sin * (y - pivot.y), sin * (x - pivot.x) + cos * (y - pivot.y));
		return rotated + pivot;
	}

	public Vec2 Normal() {
		return new Vec2(-y/this.Length(),x/this.Length());
	}

	public static Vec2 operator +(Vec2 left, Vec2 right) {
		return new Vec2 (left.x + right.x, left.y + right.y);
	}

	public static Vec2 operator -(Vec2 left, Vec2 right) {
		return new Vec2 (left.x - right.x, left.y - right.y);
	}

	public static Vec2 operator *(Vec2 v, float scalar) {
		return new Vec2 (v.x * scalar, v.y * scalar);
	}

	public static Vec2 operator *(float scalar, Vec2 v) {
		return new Vec2 (v.x * scalar, v.y * scalar);
	}

    public static Vec2 operator *(Vec2 left, Vec2 right)
    {
        return new Vec2(left.x * right.x, left.y * right.y);
    }

    public static Vec2 operator /(Vec2 v, float scalar) {
		return new Vec2 (v.x / scalar, v.y / scalar);
	}
}

