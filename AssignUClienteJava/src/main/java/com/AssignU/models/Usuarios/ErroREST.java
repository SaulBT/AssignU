package com.AssignU.models.Usuarios;

public class ErroREST {
    public String error;

    public ErroREST(String error) {
        this.error = error;
    }

    public String getError() {
        return error;
    }

    public void setError(String error) {
        this.error = error;
    }
}
