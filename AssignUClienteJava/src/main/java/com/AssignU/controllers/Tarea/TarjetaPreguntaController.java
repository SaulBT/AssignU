package com.AssignU.controllers.Tarea;

import com.AssignU.models.Cuestionarios.OpcionDTO;
import com.AssignU.models.Cuestionarios.OpcionRespuestaDTO;
import com.AssignU.models.Cuestionarios.PreguntaDTO;
import com.AssignU.models.Cuestionarios.PreguntaRespuestaDTO;
import java.net.URL;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.ResourceBundle;
import javafx.scene.control.Label;
import javafx.scene.control.RadioButton;
import javafx.scene.control.ToggleGroup;
import javafx.fxml.FXML;
import javafx.fxml.Initializable;
import javafx.scene.control.Toggle;

public class TarjetaPreguntaController implements Initializable{
    @FXML
    public Label lbNumeroPregunta;
    @FXML
    public Label lbTextoPregunta;
    @FXML
    public ToggleGroup grupoRespuestas;
    @FXML
    public RadioButton lbRespuesta1;
    @FXML
    public RadioButton lbRespuesta2;
    @FXML
    public RadioButton lbRespuesta3;
    @FXML
    public RadioButton lbRespuesta4;
    private Map<RadioButton, OpcionDTO> mapaOpciones;
    
    @Override
    public void initialize(URL url, ResourceBundle rb) {
        grupoRespuestas = new ToggleGroup();
        lbRespuesta1.setToggleGroup(grupoRespuestas);
        lbRespuesta2.setToggleGroup(grupoRespuestas);
        lbRespuesta3.setToggleGroup(grupoRespuestas);
        lbRespuesta4.setToggleGroup(grupoRespuestas);
        mapaOpciones = new HashMap<>();
    }
    
    public void cargarPregunta(PreguntaDTO pregunta){
        lbTextoPregunta.setText(pregunta.texto);
        if(pregunta.tipo.equals("verdadero_falso")){
            llenarPreguntaVerdaderoFalso(pregunta.opciones);
        } else if(pregunta.tipo.equals("opcion_multiple")){
            llenarPreguntaOpcionMultiple(pregunta.opciones);
        }
    }
    
    private void configurarPreguntaVerdaderoFalso(){
        lbRespuesta1.setText("Verdadero");
        lbRespuesta2.setText("Falso");
        lbRespuesta3.setVisible(false);
        lbRespuesta4.setVisible(false);
    }
    
    public void cambiarNumeroPregunta(int numeroPregunta){
        lbNumeroPregunta.setText(numeroPregunta + ":");
    }
    
    private void llenarPreguntaVerdaderoFalso(List<OpcionDTO> opciones){
        configurarPreguntaVerdaderoFalso();
        mapaOpciones.clear();
        mapaOpciones.put(lbRespuesta1, opciones.get(0));
        mapaOpciones.put(lbRespuesta2, opciones.get(1));
    }
    
    private void llenarPreguntaOpcionMultiple(List<OpcionDTO> opciones){
        List<RadioButton> botones = Arrays.asList(lbRespuesta1, lbRespuesta2, lbRespuesta3, lbRespuesta4);
        mapaOpciones.clear();
        for (int i = 0; i < opciones.size() && i < 4; i++) {
            OpcionDTO opcion = opciones.get(i);
            RadioButton boton = botones.get(i);
            boton.setText(opcion.getTexto());
            boton.setVisible(true);
            mapaOpciones.put(boton, opcion);
        }
    }
    
    public boolean verificarSeleccion(){
        restaurarCampos();
        if (grupoRespuestas.getSelectedToggle() == null) {
            List<RadioButton> botones = Arrays.asList(lbRespuesta1, lbRespuesta2, lbRespuesta3, lbRespuesta4);
            for (RadioButton radioButton : botones) {
                if (radioButton.isVisible()) {
                    radioButton.setStyle("-fx-border-color: red;");
                }
            }
            return false;
        }
        return true;
    }
    
    public void restaurarCampos(){
        List<RadioButton> botones = Arrays.asList(lbRespuesta1, lbRespuesta2, lbRespuesta3, lbRespuesta4);
        for (RadioButton radioButton : botones) {
            radioButton.setStyle("");
        }
    }
    
    public PreguntaRespuestaDTO obtenerResultadoPregunta(){
        Toggle seleccion = grupoRespuestas.getSelectedToggle();
        if (seleccion == null) return null;
        RadioButton seleccionado = (RadioButton) seleccion;
        
        OpcionDTO opcionElegida = mapaOpciones.get(seleccionado);
        OpcionRespuestaDTO respuestaSeleccionada = new OpcionRespuestaDTO(opcionElegida.getTexto());
        boolean esCorrecta = opcionElegida.isEsCorrecta();

        PreguntaRespuestaDTO resultado = new PreguntaRespuestaDTO(lbTextoPregunta.getText(), respuestaSeleccionada, esCorrecta);

        return resultado;
    }
    
    public void responderPregunta(PreguntaRespuestaDTO respuesta){
        String textoRespuesta = respuesta.opcion.texto;
        for (RadioButton radioButton : mapaOpciones.keySet()) {
            if (radioButton.getText().equals(textoRespuesta)) {
                radioButton.setSelected(true);
                break;
            }
        }
    }
    
    public void deshabilitarPregunta() {
        lbRespuesta1.setDisable(true);
        lbRespuesta2.setDisable(true);
        lbRespuesta3.setDisable(true);
        lbRespuesta4.setDisable(true);
    }
    
    public void calificarYDeshabilitar(PreguntaRespuestaDTO respuesta){
        responderPregunta(respuesta);
        
        deshabilitarPregunta();

        String textoRespuesta = respuesta.opcion.texto;
        for (RadioButton radioButton : mapaOpciones.keySet()) {
            if (radioButton.getText().equals(textoRespuesta)) {
                if (respuesta.isCorrecta()) {
                    radioButton.setStyle("-fx-text-fill: limegreen; -fx-font-weight: bold;");
                } else {
                    radioButton.setStyle("-fx-text-fill: crimson; -fx-font-weight: bold;");
                }
                break;
            }
        }
        
        for (Map.Entry<RadioButton, OpcionDTO> entry : mapaOpciones.entrySet()) {
            if (entry.getValue().isEsCorrecta()) {
                entry.getKey().setStyle("-fx-text-fill: limegreen; -fx-font-weight: bold;");
            }
        }
    }
}
