
package com.AssignU.utils;

public class ExcepcionHTTP extends Exception {
    private final int codigo;

    public ExcepcionHTTP(int codigo, String mensaje) {
        super(mensaje);
        this.codigo = codigo;
    }

    public int getCodigo() {
        return codigo;
    }
}
