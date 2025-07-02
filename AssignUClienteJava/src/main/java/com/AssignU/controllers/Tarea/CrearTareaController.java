package com.AssignU.controllers.Tarea;

import com.AssignU.controllers.Clase.ClaseController;
import com.AssignU.models.Clases.ClaseDTO;
import com.AssignU.servicios.ServicioClases;
import com.AssignU.utils.Constantes;
import com.AssignU.utils.IFormulario;
import com.AssignU.utils.Navegador;
import com.AssignU.utils.Utils;
import java.util.HashMap;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.scene.control.Alert;
import javafx.scene.control.Button;
import javafx.scene.control.ComboBox;
import javafx.scene.control.DatePicker;
import javafx.scene.control.Label;
import javafx.scene.control.TextField;
import javafx.scene.input.MouseEvent;

public class CrearTareaController implements IFormulario{
    public TextField tfNombreTarea;
    public DatePicker dpFechaLimite;
    public Label lbNombreArchivo;
    @FXML
    private Button btnCancelar;
    @FXML
    private Button btnAceptar;
    @FXML
    private ComboBox<String> cbTipoPregunta;
    private int idClase;

    public void cargarValores(int idClase){
        this.idClase = idClase;
    }
    
    @FXML
    public void btnSeleccionarArchivo(ActionEvent actionEvent) {
    }

    public void cbAgregarPregunta(ActionEvent actionEvent) {
    }

    @FXML
    public void btnEliminar(ActionEvent actionEvent) {
    }

    @FXML
    public void btnCrear(ActionEvent actionEvent) {
        volverAClase();
    }

    @Override
    public boolean verificarCampos() {
        throw new UnsupportedOperationException("Not supported yet."); // Generated from nbfs://nbhost/SystemFileSystem/Templates/Classes/Code/GeneratedMethodBody
        //verificar campos de cada tarjeta, probablemente llamando a su propio método verificarCampos()
    }

    @Override
    public void restaurarCampos() {
        tfNombreTarea.setStyle("-fx-border-color: black");
        dpFechaLimite.setStyle("-fx-border-color: black");
        //Quitar borde
        lbNombreArchivo.setStyle("");
    }

    @FXML
    public void btnLbVolver(MouseEvent mouseEvent) {
        volverAClase();
    }

    private void volverAClase() {
        limpiarCampos();
        HashMap<String, Object> respuesta = ServicioClases.obtenerClase(idClase);
        if (!(boolean) respuesta.get(Constantes.KEY_ERROR)) {
            ClaseDTO claseDto = (ClaseDTO)respuesta.get(Constantes.KEY_RESPUESTA);
            Navegador.cambiarVentana(
                lbNombreArchivo.getScene(),
                "/views/Clase/clase.fxml",
                claseDto.nombreClase,
                controller -> ((ClaseController) controller).cargarValores(claseDto)
            );
        } else {
            Utils.mostrarVentana("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
        }
    }

    @Override
    public void limpiarCampos() {
        throw new UnsupportedOperationException("Not supported yet."); // Generated from nbfs://nbhost/SystemFileSystem/Templates/Classes/Code/GeneratedMethodBody
    }

    @FXML
    private void cbAagregarPregunta(ActionEvent event) {
    }
}
