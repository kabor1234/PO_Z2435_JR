import static java.lang.Math.pow;
import static java.lang.Math.sqrt;

public class Calculator {
    double distance(Point a, Point b) {
        return sqrt(pow((b.getX()-a.getX()),2)+(pow(b.getY()-a.getY(),2)));
    }

    double distanceX(Point a, Point b) {
        return sqrt(pow((b.getX()-a.getX()),2));
    }

    double distanceY(Point a, Point b) {
        return sqrt(pow((b.getY()-a.getY()),2));
    }

    double calculateAreaSum(){
        return 0;
    }

    double calculatePerimeter(Shape shape1, Shape shape2) {
        return shape1.area()+shape2.area();

    }

}
