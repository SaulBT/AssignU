
package com.AssignU.controllers.Tarea;

import com.AssignU.controllers.Tarea.CrearTareaController;
import com.AssignU.servicios.ServicioArchivos;
import com.AssignU.utils.Utils;
import java.io.File;
import java.io.IOException;
import java.net.URL;
import java.util.ResourceBundle;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.fxml.Initializable;
import javafx.scene.control.Alert;
import javafx.scene.control.Button;
import javafx.scene.control.Label;
import javafx.scene.image.Image;
import javafx.scene.image.ImageView;
import javafx.stage.FileChooser;
import javafx.stage.Stage;

public class CargarArchivoController {

    @FXML
    private Label lbTextoArchivo;
    @FXML
    private ImageView ivArchivo;
    @FXML
    private Button btnDescargar;
    
    private String nombreArchivoPath;
    private String tipoArchivo = "";
    private CrearTareaController controladorPadre;
    private ServicioArchivos servicioArchivos = new ServicioArchivos();
    private boolean esEdicion = false;
    private int idTarea = 0;
    
    public void cargarValores(String nombreArchivoPath, boolean esEdicion, int idTarea, CrearTareaController controladorPadre){
        this.nombreArchivoPath = nombreArchivoPath;
        this.controladorPadre = controladorPadre;
        this.esEdicion = esEdicion;
        configurarEdicion(idTarea);
        lbTextoArchivo.setText(Utils.obtenerNombreArchivo(nombreArchivoPath));
    }

    public void configurarEdicion(int idTarea){
        if (esEdicion){
            this.idTarea = idTarea;
            btnDescargar.setVisible(true);
            btnDescargar.setDisable(false);
            cargarImagen(Utils.obtenerNombreArchivo(nombreArchivoPath));
        } else {
            btnDescargar.setVisible(false);
            btnDescargar.setDisable(true);
        }
    }
    
    private void cargarImagen(String nombreArchivo){
        Image imagen;
        obtenerExtensionArchivo(nombreArchivo);
        switch(tipoArchivo){
            case "doc":
                imagen = new Image(getClass().getResource("/recursos/DOC File Icon.png").toString());
                ivArchivo.setImage(imagen);
                lbTextoArchivo.setText(nombreArchivo);
                break;
            case "docx":
                imagen = new Image(getClass().getResource("/recursos/DOCX File Icon.png").toString());
                ivArchivo.setImage(imagen);
                lbTextoArchivo.setText(nombreArchivo);
                break;
            case "pdf":
                imagen = new Image(getClass().getResource("/recursos/PDF File Icon.png").toString());
                ivArchivo.setImage(imagen);
                lbTextoArchivo.setText(nombreArchivo);
                break;
            case "":
                imagen = new Image(getClass().getResource("/recursos/Upload File Icon.png").toString());
                ivArchivo.setImage(imagen);
                break;
            default:
                Utils.mostrarAlerta("Error Carga Archivo", "El formato del archivo seleccionado es inválido.", Alert.AlertType.WARNING);
                break;
        }
    }
    
    public void obtenerExtensionArchivo(String nombreArchivo){
        if(nombreArchivo == null || nombreArchivo.isEmpty()){
            tipoArchivo = "";
        }
        int posicionUltimoPunto = nombreArchivo.lastIndexOf(".");
        if(posicionUltimoPunto == -1 || posicionUltimoPunto == nombreArchivo.length() -1){
            tipoArchivo = "";
        }
        tipoArchivo = nombreArchivo.substring(posicionUltimoPunto + 1);
    }

    @FXML
    private void btnClicSubir(ActionEvent event) {
        FileChooser dialogoSeleccion = new FileChooser();
        dialogoSeleccion.setTitle("Seleccionar Archivo");
        String etiquetaTipoDato = "Archivos de Texto(*.pdf, *.doc, *.docx)";
        FileChooser.ExtensionFilter filtroDocumentos = new FileChooser.ExtensionFilter(etiquetaTipoDato, "*.pdf", "*.doc", "*.docx");
        dialogoSeleccion.getExtensionFilters().add(filtroDocumentos);
        
        File archivoSeleccionado = dialogoSeleccion.showOpenDialog(lbTextoArchivo.getScene().getWindow());

        if (archivoSeleccionado != null) {
            nombreArchivoPath = archivoSeleccionado.getAbsolutePath();
            String nombreArchivo = archivoSeleccionado.getName();
            lbTextoArchivo.setText(nombreArchivo);
            cargarImagen(nombreArchivo);
        }
    }

    @FXML
    private void btnClicDescargar(ActionEvent event) {
        try {
            servicioArchivos.descargarArchivo(idTarea, "downloads/" + nombreArchivoPath);
        } catch (IOException e) {
            Utils.mostrarAlerta("Descarga fallida", "No se pudo descargar el archivo", Alert.AlertType.ERROR);
        }

    }

    @FXML
    private void btnClicVolver(ActionEvent event) {
        cerrarVentana();
    }

    private void cerrarVentana() {
        controladorPadre.setPathArchivo(nombreArchivoPath);
        controladorPadre.setTipoArchivo(tipoArchivo);
        System.out.println("Tipo: " + tipoArchivo);
        Stage stage = (Stage) lbTextoArchivo.getScene().getWindow();
        stage.close();
    }
}
