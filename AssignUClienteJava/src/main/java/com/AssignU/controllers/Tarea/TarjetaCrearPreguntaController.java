package com.AssignU.controllers.Tarea;

import com.AssignU.models.Cuestionarios.OpcionDTO;
import com.AssignU.models.Cuestionarios.PreguntaDTO;
import com.AssignU.utils.IFormulario;
import com.AssignU.utils.Utils;
import java.util.Arrays;
import java.util.List;
import javafx.scene.control.Alert;
import javafx.scene.control.CheckBox;
import javafx.scene.control.Label;
import javafx.scene.control.TextField;
import javafx.scene.input.MouseEvent;
import javafx.scene.layout.HBox;

public class TarjetaCrearPreguntaController implements IFormulario{
    private Label lbNumeroPregunta;
    private TextField tfTextoPregunta;
    
    private HBox hbRespuesta1;
    private TextField tfTextoRespuesta1;
    private CheckBox cbCorrecta1;
    
    private HBox hbRespuesta2;
    private TextField tfTextoRespuesta2;
    private CheckBox cbCorrecta2;
    
    private HBox hbRespuesta3;
    private TextField tfTextoRespuesta3;
    private CheckBox cbCorrecta3;
    
    private HBox hbRespuesta4;
    private TextField tfTextoRespuesta4;
    private CheckBox cbCorrecta4;
    
    private int tipoPregunta;
    private CrearTareaController controladorPadre;
    
    public void cargarPreguntaVacia(String tipoPregunta, CrearTareaController controladorPadre){
        this.controladorPadre = controladorPadre;
        switch (tipoPregunta) {
            case "Opción Múltiple" -> {
                this.tipoPregunta = 1;
            }
            case "Verdadero y Falso" -> {
                this.tipoPregunta = 2;
                configurarPreguntaVerdaderoFalso();
            }
            default -> Utils.mostrarAlerta("Error", "Tipo de pregunta no aceptado.", Alert.AlertType.ERROR);
        }
    }
    
    public void cargarPregunta(PreguntaDTO pregunta, CrearTareaController controladorPadre){
        this.controladorPadre = controladorPadre;
        tfTextoPregunta.setText(pregunta.texto);
        if(pregunta.tipo.equals("verdadero_falso")){
            
        } else if(pregunta.tipo.equals("opcion_multiple")){
            
        }
    }
    
    private void configurarPreguntaVerdaderoFalso(){
        tfTextoRespuesta1.setText("Verdadero");
        tfTextoRespuesta1.setDisable(true);
        tfTextoRespuesta2.setText("Falso");
        tfTextoRespuesta2.setDisable(true);
        hbRespuesta3.setVisible(false);
        hbRespuesta4.setVisible(false);
    }
    
    public void cambiarNumeroPregunta(int numeroPregunta){
        lbNumeroPregunta.setText(numeroPregunta + ":");
    }
    
    public PreguntaDTO obtenerInformacionPregunta(){
        PreguntaDTO pregunta;
        if(verificarCampos() && verificarRespuestaSeleccionada()){
            List<OpcionDTO> opciones = null;
            OpcionDTO opcion1 = new OpcionDTO(tfTextoRespuesta1.getText(), cbCorrecta1.isSelected());
            OpcionDTO opcion2 = new OpcionDTO(tfTextoRespuesta2.getText(), cbCorrecta2.isSelected());
            opciones.add(opcion1);
            opciones.add(opcion2);
            if(tipoPregunta != 1){
                pregunta = new PreguntaDTO(tfTextoPregunta.getText(), "verdadero_falso", opciones);
            } else {
                OpcionDTO opcion3 = new OpcionDTO(tfTextoRespuesta3.getText(), cbCorrecta3.isSelected());
                OpcionDTO opcion4 = new OpcionDTO(tfTextoRespuesta4.getText(), cbCorrecta4.isSelected());
                opciones.add(opcion3);
                opciones.add(opcion4);
                pregunta = new PreguntaDTO(tfTextoPregunta.getText(), "opcion_multiple", opciones);
            }
        } else {
            Utils.mostrarAlerta("Error", "Faltan llenar campos en pregunta " + lbNumeroPregunta.getText() + ".", Alert.AlertType.ERROR);
            pregunta = null;
        }
        return pregunta;
    }

    public void btnLbBorrarPregunta(MouseEvent mouseEvent) {
        limpiarCampos();
        borrarPregunta();
    }
    
    public void borrarPregunta(){
        controladorPadre.eliminarPregunta(this);
    }

    @Override
    public boolean verificarCampos() {
        restaurarCampos();
        boolean error = true;
        if(tfTextoPregunta.getText().isBlank()){
            tfTextoPregunta.setStyle("-fx-border-color: red");
            error = false;
        }
        if(tfTextoRespuesta1.getText().isBlank()){
            tfTextoRespuesta1.setStyle("-fx-border-color: red");
            error = false;
        }
        if(tfTextoRespuesta2.getText().isBlank()){
            tfTextoRespuesta2.setStyle("-fx-border-color: red");
            error = false;
        }
        if(tipoPregunta == 1){
            if(tfTextoRespuesta3.getText().isBlank()){
                tfTextoRespuesta3.setStyle("-fx-border-color: red");
                error = false;
            }
            if(tfTextoRespuesta4.getText().isBlank()){
                tfTextoRespuesta4.setStyle("-fx-border-color: red");
                error = false;
            }
        }
        return error;
    }
    
    public boolean verificarRespuestaSeleccionada(){
        List<CheckBox> checkboxes = tipoPregunta != 1
            ? Arrays.asList(cbCorrecta1, cbCorrecta2)
            : Arrays.asList(cbCorrecta1, cbCorrecta2, cbCorrecta3, cbCorrecta4);
        
        boolean seleccionado = checkboxes.stream().anyMatch(CheckBox::isSelected);
        if (!seleccionado) {
            checkboxes.forEach(cb -> cb.setStyle("-fx-border-color: red; -fx-border-width: 2px;"));
        }
        return seleccionado;
    }

    @Override
    public void restaurarCampos() {
        tfTextoPregunta.setStyle("-fx-border-color: black");
        tfTextoRespuesta1.setStyle("-fx-border-color: black");
        tfTextoRespuesta2.setStyle("-fx-border-color: black");
        tfTextoRespuesta3.setStyle("-fx-border-color: black");
        tfTextoRespuesta4.setStyle("-fx-border-color: black");
        List<CheckBox> checkboxes = Arrays.asList(cbCorrecta1, cbCorrecta2, cbCorrecta3, cbCorrecta4);
        checkboxes.forEach(cb -> cb.setStyle(""));
    }

    @Override
    public void limpiarCampos() {
        tfTextoPregunta.clear();
        tfTextoRespuesta1.clear();
        tfTextoRespuesta2.clear();
        tfTextoRespuesta3.clear();
        tfTextoRespuesta4.clear();
    }
}
