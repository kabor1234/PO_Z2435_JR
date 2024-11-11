import java.awt.*;
import java.util.Scanner;

public class Main {
    public static void main(String[] args) {
        Calculator calculator = new Calculator();


        Point pointA = new Point();
        Point pointB = new Point();
        Point pointC = new Point();
        Point pointD = new Point();
        Point pointR1 = new Point();
        Point pointR2 = new Point();

        pointA.setX(1); //asdasdas
        pointA.setY(1);

        pointB.setX(1);
        pointB.setY(1);

        pointC.setX(1);
        pointC.setY(1);

        pointD.setX(1);
        pointD.setY(1);

        Square square = new Square(pointA, pointB, pointC, pointD);

        Triangle triangle = new Triangle(pointA, pointB, pointC);

        double sum = calculator.calculatePerimeter(square, triangle);


        pointR1.setX(0);
        pointR1.setY(0);

        Circle circle = new Circle(pointR1, 0.5);

    }
}

//hgfgfhgjbbnb

