import java.util.Scanner;

public class Main {
    public static void main(String[] args) {
        Calculator calculator = new Calculator();


        Point pointA = new Point();
        Point pointB = new Point();
        Point pointC = new Point();
        Point pointD = new Point();
        Point pointE = new Point();
        Circle circle = new Circle(pointE, 10);

        pointA.setX(5);
        pointA.setY(10);

        pointB.setX(3);
        pointB.setY(7);

        pointC.setX(4);
        pointC.setY(12);

        pointD.setX(5);
        pointD.setY(6);

        pointE.setX(4);
        pointE.setY(7);

        System.out.println("Point A: " + pointA.getX() + "; " + pointA.getY());
        System.out.println("Point B: " + pointB.getX() + "; " + pointB.getY());
        System.out.println("Distance between points: " + calculator.distance(pointA, pointB));
        System.out.println("Distance between x values of points: " + calculator.distanceX(pointA, pointB));
        System.out.println("Distance between y values of points: " + calculator.distanceY(pointA, pointB));
        System.out.println("Circumference of circle: " + circle.area());
        System.out.println("Area of circle: " + circle.circumference());

    }
}

