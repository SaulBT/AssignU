
package com.AssignU.controllers.Tarea;

import com.AssignU.controllers.Tarea.CrearTareaController;
import com.AssignU.utils.Utils;
import java.io.File;
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
    private CrearTareaController controladorPadre;
    
    public void cargarValores(String nombreArchivoPath, boolean esEdicion, CrearTareaController controladorPadre){
        this.nombreArchivoPath = nombreArchivoPath;
        this.controladorPadre = controladorPadre;
        btnDescargar.setDisable(!esEdicion);
        btnDescargar.setVisible(esEdicion);
        lbTextoArchivo.setText(Utils.obtenerNombreArchivo(nombreArchivoPath));
        cargarImagen(Utils.obtenerNombreArchivo(nombreArchivoPath));
    }
    
    private void cargarImagen(String nombreArchivo){
        Image imagen;
        switch(obtenerExtensionArchivo(nombreArchivo)){
            case "doc":
                imagen = new Image(getClass().getResource("/Resources/recursos/DOC File Icon.png").toString());
                ivArchivo.setImage(imagen);
                lbTextoArchivo.setText(nombreArchivo);
                break;
            case "docx":
                imagen = new Image(getClass().getResource("/Resources/recursos/DOCX File Icon.png").toString());
                ivArchivo.setImage(imagen);
                lbTextoArchivo.setText(nombreArchivo);
                break;
            case "pdf":
                imagen = new Image(getClass().getResource("/Resources/recursos/PDF File Icon.png").toString());
                ivArchivo.setImage(imagen);
                lbTextoArchivo.setText(nombreArchivo);
                break;
            case "":
                imagen = new Image(getClass().getResource("/Resources/recursos/Upload File Icon.png").toString());
                ivArchivo.setImage(imagen);
                break;
            default:
                Utils.mostrarAlerta("Error Carga Archivo", "El formato del archivo seleccionado es inválido.", Alert.AlertType.WARNING);
                break;
        }
    }
    
    public String obtenerExtensionArchivo(String nombreArchivo){
        if(nombreArchivo == null || nombreArchivo.isEmpty()){
            return "";
        }
        int posicionUltimoPunto = nombreArchivo.lastIndexOf(".");
        if(posicionUltimoPunto == -1 || posicionUltimoPunto == nombreArchivo.length() -1){
            return "";
        }
        return nombreArchivo.substring(posicionUltimoPunto + 1);
    }

    @FXML
    private void btnClicSubir(ActionEvent event) {
        FileChooser dialogoSeleccion = new FileChooser();
        dialogoSeleccion.setTitle("Seleccionar Archivo");
        String etiquetaTipoDato = "Archivos de texto(*.pdf, *.doc, *.docx)";
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
        //pendiente
        //Llamar servicio o guardar archivo que ya tenía?
    }

    @FXML
    private void btnClicVolver(ActionEvent event) {
        cerrarVentana();
    }

    private void cerrarVentana() {
        controladorPadre.setPathArchivo(nombreArchivoPath);
        Stage stage = (Stage) lbTextoArchivo.getScene().getWindow();
        stage.close();
    }
}
