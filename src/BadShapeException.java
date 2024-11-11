public class BadShapeException extends Exception {
    public BadShapeException(String message) {
        super(message);
        System.err.println("Błąd: " + message); // Wyświetlanie komunikatu błędu na konsoli
    }
}