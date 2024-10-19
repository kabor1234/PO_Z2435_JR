public class Circle{

    private Point center;
    private double diameter;

    public Circle(Point center, double diameter){
        this.center = center;
        this.diameter = diameter;
    }

    double area(){
        double radius = diameter/2;
        return Math.PI * radius * radius;
    }

    double circumference(){
        return diameter*Math.PI;
    }


}