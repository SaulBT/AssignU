package com.AssignU.controllers.Tarea;

import com.AssignU.controllers.Clase.ClaseController;
import com.AssignU.models.Clases.ClaseDTO;
import com.AssignU.models.Cuestionarios.CuestionarioDTO;
import com.AssignU.models.Cuestionarios.PreguntaDTO;
import com.AssignU.models.Tareas.CrearTareaDTO;
import com.AssignU.models.Tareas.EditarTareaDTO;
import com.AssignU.models.Tareas.TareaDTO;
import com.AssignU.servicios.ServicioClases;
import com.AssignU.servicios.ServicioCuestionarios;
import com.AssignU.servicios.ServicioTareas;
import com.AssignU.utils.Constantes;
import com.AssignU.utils.IFormulario;
import com.AssignU.utils.Navegador;
import com.AssignU.utils.Utils;
import java.io.IOException;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import javafx.beans.value.ChangeListener;
import javafx.beans.value.ObservableValue;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.control.Alert;
import javafx.scene.control.Button;
import javafx.scene.control.ComboBox;
import javafx.scene.control.DateCell;
import javafx.scene.control.DatePicker;
import javafx.scene.control.Label;
import javafx.scene.control.TextField;
import javafx.scene.input.MouseEvent;
import javafx.scene.layout.StackPane;
import javafx.scene.layout.VBox;

public class CrearTareaController implements IFormulario{
    @FXML
    public TextField tfNombreTarea;
    @FXML
    public DatePicker dpFechaLimite;
    @FXML
    public Label lbNombreArchivo;
    @FXML
    private Button btnCancelar;
    @FXML
    private Button btnAceptar;
    @FXML
    private ComboBox<String> cbTipoPregunta;
    private int idClase;
    @FXML
    private VBox vbPreguntas;
    
    private List<TarjetaCrearPreguntaController> listaControladoresPreguntas;
    
    private boolean esEdicion;
    private String nombreArchivoPath;
    private TareaDTO tareaDto;

    // C R E A C I Ó N
    public void cargarValores(int idClase){
        this.tareaDto = null;
        this.idClase = idClase;
        this.esEdicion = false;
        this.nombreArchivoPath = ""; 
        lbNombreArchivo.setText("Ningún archivo seleccionado");
        btnAceptar.setText("Crear");
        btnCancelar.setText("Cancelar");
        listaControladoresPreguntas  = new ArrayList<>();
        configurarTipoPregunta();
        configurarDatePicker();
    }
    
    public CrearTareaDTO obtenerTarea(){
        LocalDate fechaDatePicker = dpFechaLimite.getValue();
        LocalDateTime fecha = fechaDatePicker.plusDays(1).atStartOfDay().minusNanos(1);
        List<PreguntaDTO> preguntas = new ArrayList<>();
        for (int i = 0; i < listaControladoresPreguntas.size(); i++) {
            TarjetaCrearPreguntaController controlador = listaControladoresPreguntas.get(i);
            PreguntaDTO pregunta = controlador.obtenerInformacionPregunta();
            preguntas.add(pregunta);
        }
        CuestionarioDTO cuestionario = new CuestionarioDTO(1, preguntas);
        CrearTareaDTO tarea = new CrearTareaDTO(idClase, tfNombreTarea.getText(), fecha,cuestionario);
        return tarea;
    }
    
    public void crearTarea(){
        HashMap<String, Object> respuesta = ServicioTareas.crearTarea(obtenerTarea());
        //llamar a servicio archivos? enviar nombreArchivoPath
        if (!(boolean) respuesta.get(Constantes.KEY_ERROR)) {
            TareaDTO tarea = (TareaDTO) respuesta.get(Constantes.KEY_RESPUESTA);
            Utils.mostrarAlerta("Éxito", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.INFORMATION);
            volverAClase();
        } else {
            Utils.mostrarAlerta("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
        }
    }
    
    // E D I C I Ó N
    public void cargarValoresEdicion(TareaDTO tarea){
        this.tareaDto = tarea;
        this.idClase = tarea.idClase;
        this.esEdicion = true;
        listaControladoresPreguntas  = new ArrayList<>();
        configurarDatePicker();
        LocalDateTime fechaLimite = tarea.fechaLimite;
        LocalDate fechaDatePicker = fechaLimite.toLocalDate();
        dpFechaLimite.setValue(fechaDatePicker);
        //this.nombreArchivoPath = obtenerNombreArchivo; SERVICIO ARCHIVOS
        //lbNombreArchivo.setText(nombreArchivoPath)
        HashMap<String, Object> respuesta = ServicioCuestionarios.obtenerCuestionario(tarea.idTarea);
        if (!(boolean) respuesta.get(Constantes.KEY_ERROR)) {
            CuestionarioDTO cuestionario = (CuestionarioDTO) respuesta.get(Constantes.KEY_RESPUESTA);
            desplegarPreguntas(cuestionario.preguntas);
        } else {
            Utils.mostrarAlerta("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
        }
        configurarTipoPregunta();
    }
    
    public void desplegarPreguntas(List<PreguntaDTO> preguntas){
        try {
            vbPreguntas.getChildren().clear();
            for (PreguntaDTO pregunta : preguntas){
                FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/Tarea/tarjetaCrearPregunta.fxml"));
                VBox tarjeta = loader.load();

                TarjetaCrearPreguntaController controller = loader.getController();
                listaControladoresPreguntas.add(controller);

                vbPreguntas.getChildren().add(tarjeta);

                actualizarNumeracionPreguntas();

                controller.cargarPregunta(pregunta, this);
            }
        } catch (IOException ex) {
            System.out.println("Error: " + ex.getMessage());
        }
    }
    
    public EditarTareaDTO obtenerTareaEdicion(){
        LocalDate fechaDatePicker = dpFechaLimite.getValue();
        LocalDateTime fecha = fechaDatePicker.plusDays(1).atStartOfDay().minusNanos(1);
        List<PreguntaDTO> preguntas = new ArrayList<>();
        for (int i = 0; i < listaControladoresPreguntas.size(); i++) {
            TarjetaCrearPreguntaController controlador = listaControladoresPreguntas.get(i);
            PreguntaDTO pregunta = controlador.obtenerInformacionPregunta();
            preguntas.add(pregunta);
        }
        CuestionarioDTO cuestionario = new CuestionarioDTO(1, preguntas);
        EditarTareaDTO tareaEdicion = new EditarTareaDTO(idClase, tfNombreTarea.getText(), fecha,cuestionario);
        return tareaEdicion;
    }
    
    public void guardarTarea(){
        HashMap<String, Object> respuesta = ServicioTareas.editarTarea(obtenerTareaEdicion(), tareaDto.idTarea);
        //Si nombreArchivoPath cambió...
        //llamar a servicio archivos? enviar nombreArchivoPath
        if (!(boolean) respuesta.get(Constantes.KEY_ERROR)) {
            TareaDTO tarea = (TareaDTO) respuesta.get(Constantes.KEY_RESPUESTA);
            Utils.mostrarAlerta("Éxito", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.INFORMATION);
            volverAClase();
        } else {
            Utils.mostrarAlerta("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
        }
    }
    
    // G E N E R A L
    @FXML
    private void clicAceptar(ActionEvent event) {
        if(verificarCampos()){
            if(verificarSeleccionRespuestas()){
                if(esEdicion){
                    guardarTarea();
                } else {
                    crearTarea();
                }
            } else {
                Utils.mostrarAlerta("Selección Vacía", "Seleccione al menos una respuesta correcta por pregunta.", Alert.AlertType.ERROR);
            }
        } else {
            Utils.mostrarAlerta("Campos Vacíos", "Favor de llenar todos los campos.", Alert.AlertType.ERROR);
        }
    }
    
    @FXML
    private void clicCancelar(ActionEvent event) {
        boolean cerrarVentana;
        if(esEdicion){
            cerrarVentana = Utils.mostrarAlertaConfirmacion("Eliminar Tarea", "¿Está seguro de que desea eliminar la tarea?\n Se borrará permanentemente.");
            if(cerrarVentana){
                HashMap<String, Object> respuesta = ServicioTareas.borrarTarea(tareaDto.idTarea);
                if (!(boolean) respuesta.get(Constantes.KEY_ERROR)) {
                    Utils.mostrarAlerta("Éxito", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.INFORMATION);
                } else {
                    Utils.mostrarAlerta("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
                }
            }
        } else {
            cerrarVentana = Utils.mostrarAlertaConfirmacion("Cancelar Tarea", "¿Está seguro de que desea cancelar?\n Se borrarán todos los campos.");
        }
        if(cerrarVentana){
            volverAClase();
        }
    }
    
    @FXML
    public void btnSeleccionarArchivo(ActionEvent actionEvent) {
        Navegador.abrirVentanaModal(
            "/views/Tarea/cargarArchivo.fxml", 
            "Cargar Archivos", 
            controller -> ((CargarArchivoController) controller).cargarValores(
                    lbNombreArchivo.getText(), 
                    esEdicion,
                    this)
        );
    }
    
    public void setPathArchivo(String pathArchivo){
        nombreArchivoPath = pathArchivo;
        lbNombreArchivo.setText(Utils.obtenerNombreArchivo(pathArchivo));
    }

    @FXML
    public void btnLbVolver(MouseEvent mouseEvent) {
        if(Utils.mostrarAlertaConfirmacion("Volver", "¿Está seguro de que desea volver?\n Se borrarán todos los campos.")){
            volverAClase();
        }
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
            Utils.mostrarAlerta("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
        }
    }
    
    // V E R I F I C A C I O N E S
    @Override
    public boolean verificarCampos() {
        restaurarCampos();
        boolean error = true;
        if(tfNombreTarea.getText().isBlank()){
            tfNombreTarea.setStyle("-fx-border-color: red");
            error = false;
        }
        if(dpFechaLimite.getValue() == null){
            dpFechaLimite.setStyle("-fx-border-color: red");
            error = false;
        }
        
        boolean esNombreVacio = lbNombreArchivo.getText().equals("Ningún archivo seleccionado");
        boolean noHuboCambios = lbNombreArchivo.getText().equals(Utils.obtenerNombreArchivo(nombreArchivoPath));
        if (esNombreVacio || (!esEdicion && noHuboCambios)) {
            lbNombreArchivo.setStyle("-fx-border-color: red");
            error = false;
        }
        
        for (int i = 0; i < listaControladoresPreguntas.size(); i++) {
            TarjetaCrearPreguntaController controlador = listaControladoresPreguntas.get(i);
            if(controlador.verificarCampos()){
                return false;
            }
        }
        return error;
    }

    @Override
    public void restaurarCampos() {
        tfNombreTarea.setStyle("-fx-border-color: black");
        dpFechaLimite.setStyle("-fx-border-color: black");
        lbNombreArchivo.setStyle("");
    }
    
    private boolean verificarSeleccionRespuestas(){
        for (int i = 0; i < listaControladoresPreguntas.size(); i++) {
            TarjetaCrearPreguntaController controlador = listaControladoresPreguntas.get(i);
            if(controlador.verificarRespuestaSeleccionada()){
                return false;
            }
        }
        return true;
    }

    @Override
    public void limpiarCampos() {
        tfNombreTarea.clear();
        dpFechaLimite.setValue(LocalDate.MIN);
        lbNombreArchivo.setText("Ningún archivo seleccionado");
        for (int i = 0; i < listaControladoresPreguntas.size(); i++) {
            TarjetaCrearPreguntaController controlador = listaControladoresPreguntas.get(i);
            controlador.limpiarCampos();
        }
    }
    
    
    // C O N F I G U R A C I O N E S
    
    private void configurarDatePicker(){
        LocalDate hoy = LocalDate.now();
        dpFechaLimite.setDayCellFactory(dp -> new DateCell() {
            @Override
            public void updateItem(LocalDate item, boolean empty) {
                super.updateItem(item, empty);
                getStyleClass().removeAll("celda-deshabilitada", "celda-seleccionada");
                if (empty) {
                    setDisable(true);
                }
                if (item.isBefore(hoy)) {
                    setDisable(true);
                    getStyleClass().add("celda-deshabilitada");
                }
                if (item.equals(dpFechaLimite.getValue())) {
                    getStyleClass().add("celda-seleccionada");
                }
            }
        });
        if (dpFechaLimite.getValue() == null || dpFechaLimite.getValue().isBefore(hoy)) {
            dpFechaLimite.setValue(hoy);
        }
    }
    
    private void configurarTipoPregunta(){
        List<String> preguntas = new ArrayList<String>() {{
            add("Opción Múltiple");
            add("Verdadero y Falso");
        }};
        ObservableList<String> observablePreguntas = FXCollections.observableArrayList(preguntas);
        cbTipoPregunta.setItems(observablePreguntas);
        
        cbTipoPregunta.valueProperty().addListener(new ChangeListener<String>(){
            @Override
            public void changed(ObservableValue<? extends String> observable, String oldValue, String newValue) {
                if(newValue != null){
                    agregarPregunta(newValue);
                }
            }
        });
        
        if (!observablePreguntas.isEmpty()) {
            cbTipoPregunta.getSelectionModel().selectFirst();
        }
    }
    
    private void agregarPregunta(String tipoPregunta){
        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/Tarea/tarjetaCrearPregunta.fxml"));
            VBox tarjeta = loader.load();
            
            TarjetaCrearPreguntaController controller = loader.getController();
            listaControladoresPreguntas.add(controller);
            
            vbPreguntas.getChildren().add(tarjeta);
            
            actualizarNumeracionPreguntas();
            
            controller.cargarPreguntaVacia(tipoPregunta, this);
        } catch (IOException ex) {
            System.out.println("Error: " + ex.getMessage());
        }
    }
    
    private void actualizarNumeracionPreguntas() {
        for (int i = 0; i < listaControladoresPreguntas.size(); i++) {
            TarjetaCrearPreguntaController controlador = listaControladoresPreguntas.get(i);
            controlador.cambiarNumeroPregunta(i + 1);
        }
    }
    
    public void eliminarPregunta(TarjetaCrearPreguntaController controller) {
        int index = listaControladoresPreguntas.indexOf(controller);
        if (index != -1) {
            listaControladoresPreguntas.remove(index);
            vbPreguntas.getChildren().remove(index);
            actualizarNumeracionPreguntas();
        }
    }
}
