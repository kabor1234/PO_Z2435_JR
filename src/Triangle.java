import static java.lang.Math.pow;

public class Triangle extends Shape {

    private Point a;
    private Point b;
    private Point c;
    private double area;
    private double circumference;

    public Triangle(Point pkt1, Point pkt2, Point pkt3) {
        this.a = pkt1;
        this.b = pkt2;
        this.c = pkt3;

        if((pkt1 == pkt2) || (pkt1 == pkt3) || (pkt2 == pkt3)) {
            throw new IllegalArgumentException("To nie jest trójkąt");
        }
    }

    @Override
    double area() {
        Calculator calc = new Calculator();
        return (0.5*calc.distance(a,b)*calc.distanceY(a,c)); // Przy zalozeniu, ze punkty podstawy trojkata leza na tej samej wysokosci
    }

    @Override
    double circumference() {
        Calculator calc = new Calculator();
        return calc.distance(a, b)+calc.distance(b, c)+calc.distance(a, c);
    }

}
