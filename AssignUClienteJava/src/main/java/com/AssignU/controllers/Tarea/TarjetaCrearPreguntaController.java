package com.AssignU.controllers.Tarea;

import com.AssignU.models.Cuestionarios.OpcionDTO;
import com.AssignU.models.Cuestionarios.PreguntaDTO;
import com.AssignU.utils.IFormulario;
import com.AssignU.utils.Utils;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import javafx.fxml.FXML;
import javafx.scene.control.Alert;
import javafx.scene.control.CheckBox;
import javafx.scene.control.Label;
import javafx.scene.control.TextField;
import javafx.scene.input.MouseEvent;
import javafx.scene.layout.HBox;

public class TarjetaCrearPreguntaController implements IFormulario{
    @FXML
    private Label lbNumeroPregunta;
    @FXML
    private TextField tfTextoPregunta;
    
    private HBox hbRespuesta1;
    @FXML
    private TextField tfTextoRespuesta1;
    @FXML
    private CheckBox cbCorrecta1;
    
    private HBox hbRespuesta2;
    @FXML
    private TextField tfTextoRespuesta2;
    @FXML
    private CheckBox cbCorrecta2;
    
    @FXML
    private HBox hbRespuesta3;
    @FXML
    private TextField tfTextoRespuesta3;
    @FXML
    private CheckBox cbCorrecta3;
    
    @FXML
    private HBox hbRespuesta4;
    @FXML
    private TextField tfTextoRespuesta4;
    @FXML
    private CheckBox cbCorrecta4;
    
    private int tipoPregunta;
    private CrearTareaController controladorPadre;
    
    public void cargarPreguntaVacia(String tipoPregunta, CrearTareaController controladorPadre){
        this.controladorPadre = controladorPadre;
        switch (tipoPregunta) {
            case "Opción Múltiple" -> this.tipoPregunta = 1;
            case "Verdadero y Falso" -> {
                this.tipoPregunta = 2;
                configurarPreguntaVerdaderoFalso();
            }
            default -> Utils.mostrarAlerta("Error", "Tipo de pregunta no aceptado.", Alert.AlertType.ERROR);
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
    
    public void cargarPregunta(PreguntaDTO pregunta, CrearTareaController controladorPadre){
        this.controladorPadre = controladorPadre;
        tfTextoPregunta.setText(pregunta.Texto);
        if(pregunta.Tipo.equals("verdadero_falso")){
            llenarPreguntaVerdaderoFalso(pregunta.Opciones);
            this.tipoPregunta = 2;
        } else if(pregunta.Tipo.equals("opcion_multiple")){
            this.tipoPregunta = 1;
            llenarPreguntaOpcionMultiple(pregunta.Opciones);
        }
    }
    
    private void llenarPreguntaVerdaderoFalso(List<OpcionDTO> opciones){
        configurarPreguntaVerdaderoFalso();
        List<CheckBox> checkBoxes = Arrays.asList(cbCorrecta1, cbCorrecta2);
        
        for (int i = 0; i < opciones.size() && i < 2; i++) {
            OpcionDTO opcion = opciones.get(i);
            checkBoxes.get(i).setSelected(opcion.isEsCorrecta());
        }
    }
    
    private void llenarPreguntaOpcionMultiple(List<OpcionDTO> opciones){
        List<TextField> textFields = Arrays.asList(tfTextoRespuesta1, tfTextoRespuesta2, tfTextoRespuesta3, tfTextoRespuesta4);
        List<CheckBox> checkBoxes = Arrays.asList(cbCorrecta1, cbCorrecta2, cbCorrecta3, cbCorrecta4);

        for (int i = 0; i < opciones.size() && i < 4; i++) {
            OpcionDTO opcion = opciones.get(i);
            textFields.get(i).setText(opcion.getTexto());
            checkBoxes.get(i).setSelected(opcion.isEsCorrecta());
        }
    }
    
    public void cambiarNumeroPregunta(int numeroPregunta){
        lbNumeroPregunta.setText(numeroPregunta + ":");
    }
    
    public PreguntaDTO obtenerInformacionPregunta(){
        if (!verificarCampos() || !verificarRespuestaSeleccionada()) {
            Utils.mostrarAlerta("Error", "Faltan llenar campos en pregunta " + lbNumeroPregunta.getText() + ".", Alert.AlertType.ERROR);
            return null;
        }

        List<OpcionDTO> opciones = new ArrayList<>();
        List<TextField> textFields = Arrays.asList(tfTextoRespuesta1, tfTextoRespuesta2, tfTextoRespuesta3, tfTextoRespuesta4);
        List<CheckBox> checkBoxes = Arrays.asList(cbCorrecta1, cbCorrecta2, cbCorrecta3, cbCorrecta4);

        int cantidad = (tipoPregunta != 1) ? 2 : 4;
        for (int i = 0; i < cantidad; i++) {
            String texto = textFields.get(i).getText().trim();
            boolean esCorrecta = checkBoxes.get(i).isSelected();
            opciones.add(new OpcionDTO(texto, esCorrecta));
        }

        String tipo = (tipoPregunta != 1) ? "verdadero_falso" : "opcion_multiple";
        String enunciado = tfTextoPregunta.getText().trim();

        return new PreguntaDTO(enunciado, tipo, opciones);
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
