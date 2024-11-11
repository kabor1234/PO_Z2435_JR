public class Circle extends Shape{

    private Point center;
    private double diameter;

    public Circle(Point center, double diameter){
        this.center = center;
        this.diameter = diameter;
        double radius = diameter / 2;
        try {
            if (radius * 2 < 1) {
                throw new BadShapeTwoException("Średnica musi być większa lub równa 1.");
            }
        } catch (BadShapeTwoException e) {
            System.err.println("Błąd: " + e.getMessage());
        }
        finally {
            System.out.println("Praca zakończona.");
        }
    }

    double area(){
        double radius = diameter/2;
        return Math.PI * radius * radius;
    }

    double circumference(){
        return diameter*Math.PI;
    }


}