import java.util.Scanner;

public class Main {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);

        int number1;
        int number2;

        System.out.println("Podaj piewsza liczbe: ");
        number1 = scanner.nextInt();
        System.out.println("Podaj druga liczbee: ");
        number2 = scanner.nextInt();

        System.out.println("Dodawanie: " + (number1+number2));
        System.out.println("Odejmowanie: " + (number1-number2));
        System.out.println("Mnozenie: " + (number1*number2));
        System.out.println("Dzielenie: " + (number1/number2));
    }
}