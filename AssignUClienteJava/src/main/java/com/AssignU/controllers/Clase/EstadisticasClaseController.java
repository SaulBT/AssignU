package com.AssignU.controllers.Clase;

import com.AssignU.models.Clases.ClaseDTO;
import com.AssignU.models.Clases.Estadisticas.AlumnoEstadisticaClaseDTO;
import com.AssignU.models.Clases.Estadisticas.EstadisticasClaseDTO;
import com.AssignU.servicios.ServicioClases;
import com.AssignU.utils.Constantes;
import com.AssignU.utils.Navegador;
import com.AssignU.utils.Utils;
import java.util.HashMap;
import javafx.beans.property.SimpleStringProperty;
import javafx.fxml.FXML;
import javafx.scene.control.*;
import javafx.scene.input.MouseEvent;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.scene.control.TableColumn;
import javafx.scene.control.TableView;
import javafx.beans.property.SimpleIntegerProperty;

public class EstadisticasClaseController {
    @FXML
    private Label lbNombreClase;
    @FXML
    private Label lbNumeroAlumnos;
    @FXML
    private Label lbNumeroTareas;
    @FXML
    private ScrollPane spInformacionClase;
    @FXML
    private TableColumn<AlumnoEstadisticaClaseDTO, String> tcAlumno;
    @FXML
    private TableColumn<AlumnoEstadisticaClaseDTO, String> tcUltimaConexion;
    @FXML
    private TableColumn<AlumnoEstadisticaClaseDTO, Integer> tcTotal;
    @FXML
    private TableColumn<AlumnoEstadisticaClaseDTO, String> tcPromedio;
    @FXML
    private TableView<AlumnoEstadisticaClaseDTO> tvDatosClase;
    
    private ClaseDTO claseDto;
    
    public void cargarValores(ClaseDTO claseDto){
        this.claseDto = claseDto;
    }
    
    public void obtenerEstadisticasClase(){
        HashMap<String, Object> respuesta = ServicioClases.obtenerEstadisticasClase(claseDto.idClase);
        if (!(boolean) respuesta.get(Constantes.KEY_ERROR)) {
            EstadisticasClaseDTO estadisticasClase = (EstadisticasClaseDTO) respuesta.get(Constantes.KEY_RESPUESTA);
            cargarEstadisticas(estadisticasClase);
        } else {
            Utils.mostrarAlerta("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
            volver();
        }
    }
    
    public void cargarEstadisticas(EstadisticasClaseDTO estadisticasClase){
        lbNombreClase.setText("Estadísticas - " + claseDto.nombreClase);
        int numeroAlumnos = estadisticasClase.alumnos.size();
        lbNumeroAlumnos.setText("" + numeroAlumnos);
        int numeroTareas = estadisticasClase.tareas.size();
        lbNumeroTareas.setText("" + numeroTareas);
        configurarEstadisticasTabla(estadisticasClase);
    }
    
    public void configurarEstadisticasTabla(EstadisticasClaseDTO estadisticasClase){
        tcAlumno.setCellValueFactory(cellData -> 
            new SimpleStringProperty(cellData.getValue().getNombreCompleto())
        );

        tcUltimaConexion.setCellValueFactory(cellData -> 
            new SimpleStringProperty(cellData.getValue().getUltimaConexion().toLocalDate().toString())
        );

        tcTotal.setCellValueFactory(cellData -> {
            int tareasCompletadas = estadisticasClase.contarTareasCompletadas(cellData.getValue().getRespuestas());
            return new SimpleIntegerProperty(tareasCompletadas).asObject();
        });

        tcPromedio.setCellValueFactory(cellData -> {
            String promedio = cellData.getValue().calcularPromedio(estadisticasClase.getTareas().size());
            return new SimpleStringProperty(promedio);
        });
        tcPromedio.setCellFactory(columna -> new TableCell<AlumnoEstadisticaClaseDTO, String>() {
            @Override
            protected void updateItem(String promedioTexto, boolean empty) {
                super.updateItem(promedioTexto, empty);
                if (empty || promedioTexto == null || promedioTexto.isBlank()) {
                    setText(null);
                    setStyle("");
                } else {
                    setText(promedioTexto);
                    try {
                        double promedio = Double.parseDouble(promedioTexto);
                        if (promedio >= 9.0) {
                            setStyle("-fx-text-fill: limegreen;");
                        } else if (promedio <= 5.9) {
                            setStyle("-fx-text-fill: crimson;");
                        } else {
                            setStyle("-fx-text-fill: black;");
                        }
                    } catch (NumberFormatException e) {
                        // Si no es un número válido, simplemente no aplicar estilo
                        setStyle("-fx-text-fill: black;");
                    }
                }
            }
        });

        ObservableList<AlumnoEstadisticaClaseDTO> datos = FXCollections.observableArrayList(estadisticasClase.getAlumnos());
        tvDatosClase.setItems(datos);
    }

    @FXML
    public void btnLbVolver(MouseEvent mouseEvent) {
        volver();
    }
    
    public void volver(){
        Navegador.cambiarVentana(
            lbNombreClase.getScene(),
            "/views/Clase/clase.fxml",
            claseDto.nombreClase,
            controller -> ((ClaseController) controller).cargarValoresDeMenu(claseDto)
        );
    }
}
