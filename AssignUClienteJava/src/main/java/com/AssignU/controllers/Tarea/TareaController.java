package com.AssignU.controllers.Tarea;

import com.AssignU.controllers.Clase.ClaseController;
import com.AssignU.models.Clases.ClaseDTO;
import com.AssignU.models.Cuestionarios.CuestionarioDTO;
import com.AssignU.models.Cuestionarios.PreguntaDTO;
import com.AssignU.models.Cuestionarios.PreguntaRespuestaDTO;
import com.AssignU.models.Cuestionarios.RespuestaDTO;
import com.AssignU.models.Tareas.TareaDTO;
import com.AssignU.models.Usuarios.Sesion;
import com.AssignU.servicios.ServicioClases;
import com.AssignU.servicios.ServicioCuestionarios;
import com.AssignU.utils.Constantes;
import com.AssignU.utils.IFormulario;
import com.AssignU.utils.Navegador;
import com.AssignU.utils.Utils;
import java.io.IOException;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.control.Alert;
import javafx.scene.control.Button;
import javafx.scene.control.Label;
import javafx.scene.input.MouseEvent;
import javafx.scene.layout.VBox;

public class TareaController{
    @FXML
    public Label lbNombreTarea;
    @FXML
    public Label lbFechaLimite;
    @FXML
    public Label lbNombreArchivo;
    @FXML
    public VBox vbCuestionario;
    @FXML
    private Button btnResponder;
    
    private List<TarjetaPreguntaController> listaControladoresPreguntas;
    private int idClase;
    private TareaDTO tareaDto;
    @FXML
    private Label lbTextoCalificacion;
    @FXML
    private Label lbCalificacion;
    
    public void cargarValores(int idClase, TareaDTO tarea){
        this.idClase = idClase;
        this.tareaDto = tarea;
        lbNombreTarea.setText(tarea.nombre);
        lbFechaLimite.setText(tarea.fechaLimite.toString());
        listaControladoresPreguntas  = new ArrayList<>();
        //Obtener nombre archivo
        //lbNombreArchivo.setText(nombre del archivo);
        cargarPreguntas(tarea);
    }
    
    public void cargarPreguntas(TareaDTO tarea){
        boolean tareaVencida = tarea.getFechaLimite().isBefore(LocalDateTime.now());
        
        HashMap<String, Object> respuestaCuestionario = ServicioCuestionarios.obtenerCuestionario(tarea.idTarea);
        if ((boolean) respuestaCuestionario.get(Constantes.KEY_ERROR)) {
            Utils.mostrarAlerta("Error", (String) respuestaCuestionario.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
            return;
        }
        CuestionarioDTO cuestionario = (CuestionarioDTO) respuestaCuestionario.get(Constantes.KEY_RESPUESTA);
        desplegarPreguntas(cuestionario.preguntas);
        
        HashMap<String, Object> respuestaAlumnoRaw = ServicioCuestionarios.obtenerRespuestaAlumno(tarea.getIdTarea());
        if (!(boolean) respuestaAlumnoRaw.get(Constantes.KEY_ERROR)) {
            RespuestaDTO respuestaAlumno = (RespuestaDTO) respuestaAlumnoRaw.get(Constantes.KEY_RESPUESTA);
            
            for (TarjetaPreguntaController controller : listaControladoresPreguntas) {
                String textoPregunta = controller.lbTextoPregunta.getText();

                PreguntaRespuestaDTO respuesta = null;
                if (respuestaAlumno != null && respuestaAlumno.getPreguntas() != null) {
                    for (PreguntaRespuestaDTO pr : respuestaAlumno.getPreguntas()) {
                        if (pr.getTexto().equals(textoPregunta)) {
                            respuesta = pr;
                            break;
                        }
                    }
                }

                if (tareaVencida) {
                    controller.deshabilitarPregunta();
                } else if (respuesta != null) {
                    if ("resuelta".equals(respuestaAlumno.getEstado())) {
                        controller.calificarYDeshabilitar(respuesta);
                    } else if ("pendiente".equals(respuestaAlumno.getEstado())) {
                        controller.responderPregunta(respuesta);
                    }
                }
            }
            if (tareaVencida || (respuestaAlumno != null && "resuelta".equals(respuestaAlumno.getEstado()))) {
                btnResponder.setDisable(true);
            }
            if (respuestaAlumno != null && "resuelta".equals(respuestaAlumno.getEstado())) {
                lbCalificacion.setText(String.valueOf(respuestaAlumno.getCalificacion()));
            } else if (tareaVencida && (respuestaAlumno == null || "pendiente".equals(respuestaAlumno.getEstado()))) {
                lbCalificacion.setText("0.0");
                btnResponder.setDisable(true);
            } else {
                lbTextoCalificacion.setVisible(false);
                lbCalificacion.setVisible(false);
            }
        } else {
            Utils.mostrarAlerta("Error", (String) respuestaAlumnoRaw.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
        }
    }
    
    public void desplegarPreguntas(List<PreguntaDTO> preguntas){
        try {
            vbCuestionario.getChildren().clear();
            for (PreguntaDTO pregunta : preguntas){
                FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/Tarea/tarjetaPregunta.fxml"));
                VBox tarjeta = loader.load();

                TarjetaPreguntaController controller = loader.getController();
                listaControladoresPreguntas.add(controller);

                vbCuestionario.getChildren().add(tarjeta);

                actualizarNumeracionPreguntas();

                controller.cargarPregunta(pregunta);
            }
        } catch (IOException ex) {
            System.out.println("Error: " + ex.getMessage());
        }
    }
    
    private void actualizarNumeracionPreguntas() {
        for (int i = 0; i < listaControladoresPreguntas.size(); i++) {
            TarjetaPreguntaController controlador = listaControladoresPreguntas.get(i);
            controlador.cambiarNumeroPregunta(i + 1);
        }
    }

    @FXML
    public void btnDescargar(ActionEvent actionEvent) {
        //llamar descargar archivo
    }

    @FXML
    private void clicResponder(ActionEvent event) {
        if(verificarCampos()){
            calificarCuestionario();
        } else {
            Utils.mostrarAlerta("Respuesta Vacía", "Favor de responder todas las preguntas.", Alert.AlertType.ERROR);
        }
    }
    
    private void calificarCuestionario(){
        HashMap<String, Object> respuesta = ServicioCuestionarios.calificarCuestionario(tareaDto.idTarea, obtenerRespuestaAlumno());
        if (!(boolean) respuesta.get(Constantes.KEY_ERROR)) {
            Utils.mostrarAlerta("Éxito", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.INFORMATION);
            volverAClase();
        } else {
            Utils.mostrarAlerta("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
        }
    }
    
    private RespuestaDTO obtenerRespuestaAlumno(){
        Sesion sesion = Sesion.getSesion();
        List<PreguntaRespuestaDTO> preguntas = new ArrayList<>();
        for (int i = 0; i < listaControladoresPreguntas.size(); i++) {
            TarjetaPreguntaController controlador = listaControladoresPreguntas.get(i);
            PreguntaRespuestaDTO pregunta = controlador.obtenerResultadoPregunta();
            if(pregunta != null){
                preguntas.add(pregunta);
            }
        }
        RespuestaDTO respuestaAlumno = new RespuestaDTO(
                sesion.getIdUsuario(),
                tareaDto.idTarea,
                "",
                0,
                preguntas
        );
        return respuestaAlumno;
    }

    @FXML
    public void btnLbVolver(MouseEvent mouseEvent) {
        if(Utils.mostrarAlertaConfirmacion("Volver", "¿Está seguro de que desea volver?\n Se guardará su progreso.")){
            guardarProgresoCuestionario();
            volverAClase();
        }
    }

    private void volverAClase() {
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
            Utils.mostrarAlerta("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
        }
    }
    
    private void guardarProgresoCuestionario(){
        HashMap<String, Object> respuesta = ServicioCuestionarios.guardarCuestionario(tareaDto.idTarea, obtenerRespuestaAlumno());
        if (!(boolean) respuesta.get(Constantes.KEY_ERROR)) {
            Utils.mostrarAlerta("Éxito", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.INFORMATION);
            volverAClase();
        } else {
            Utils.mostrarAlerta("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
        }
    }

    public boolean verificarCampos() {
        for (int i = 0; i < listaControladoresPreguntas.size(); i++) {
            TarjetaPreguntaController controlador = listaControladoresPreguntas.get(i);
            if(!controlador.verificarSeleccion()){
                return false;
            }
        }
        return false;
    }
}
