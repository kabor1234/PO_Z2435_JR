import static java.lang.Math.pow;

public class Square extends Shape {

    private Point a;
    private Point b;
    private Point c;
    private Point d;
    private double area;
    private double circumference;

    public Square(Point pkt1, Point pkt2, Point pkt3, Point pkt4) {
        this.a = pkt1;
        this.b = pkt2;
        this.c = pkt3;
        this.d = pkt4;
    }
    @Override
    double area() {
        Calculator calc = new Calculator();
        return pow(calc.distance(a, b), 2);
    }

    @Override
    double circumference() {
        Calculator calc = new Calculator();
        return calc.distance(a, b)+calc.distance(b, c)+calc.distance(c, d)+calc.distance(a, d);
    }

}
