import static java.lang.Math.pow;

public class Square extends Shape {

    private Point a;
    private Point b;
    private Point c;
    private Point d;
    private double area;
    private double circumference;

    public Square(Point pkt1, Point pkt2, Point pkt3, Point pkt4) throws BadShapeException {
        this.a = pkt1;
        this.b = pkt2;
        this.c = pkt3;
        this.d = pkt4;

        try {
            if ((pkt1 == pkt2) || (pkt1 == pkt3) || (pkt1 == pkt4) || (pkt2 == pkt3) || (pkt2 == pkt4) || (pkt3 == pkt4) || (pkt4 == pkt1)) {
                throw new BadShapeException("To nie kwadrat");
            }
        } finally {
            System.out.println("Praca zakończona.");
        }
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
