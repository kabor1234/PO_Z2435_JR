import java.awt.*;
import java.util.Scanner;

public class Main {
    public static void main(String[] args) {
        Calculator calculator = new Calculator();


        Point pointA = new Point();
        Point pointB = new Point();
        Point pointC = new Point();
        Point pointD = new Point();

        pointA.setX(1); //asdasdas
        pointA.setY(1);

        pointB.setX(5);
        pointB.setY(1);

        pointC.setX(5);
        pointC.setY(5);

        pointD.setX(1);
        pointD.setY(5);

        Square square = new Square(pointA, pointB, pointC, pointD);

        Triangle triangle = new Triangle(pointA, pointB, pointC);

        double sum = calculator.calculatePerimeter(square, triangle);

        System.out.println("Suma pól: " + sum);

    }
}

//hgfgfhgjbbnb

