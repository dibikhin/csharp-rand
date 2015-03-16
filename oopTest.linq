<Query Kind="Program" />

void Main()
{
	IAreaComputable circle = new Сircle(5);
	IAreaComputable square = new Square(5);
	Quadrilateral rhombus = new Rhombus(5, 7);
	
	circle.ComputeArea().Dump();
	square.ComputeArea().Dump();
	((IAreaComputable)rhombus).ComputeArea().Dump();
}

// Abstracts
interface IAreaComputable {
	double ComputeArea();
}

abstract class Shape : IAreaComputable {
	abstract public double ComputeArea();
}

abstract class Quadrilateral : Shape {
	abstract public override double ComputeArea();
}

// Shapes
sealed class Сircle: Shape {
	int _radius;
	
	public Сircle(int radius) {
		_radius = radius;
	}
	
	public override double ComputeArea() {
		return Math.PI * _radius * _radius;
	}
}

sealed class Square: Quadrilateral {
	int _side;
	
	public Square(int side) {
		_side = side;
	}
	
	public override double ComputeArea() {
		return _side * _side;
	}
}

sealed class Rhombus: Quadrilateral {
	int _length;
	int _height;
	
	public Rhombus(int length, int height) {
		_length = length;
		_height = height;
	}
	
	public override double ComputeArea() {
		return _length * _height;
	}
}