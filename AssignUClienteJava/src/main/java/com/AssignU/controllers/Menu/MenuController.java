package com.AssignU.controllers.Menu;

import com.AssignU.controllers.Clase.CrearUnirseAClaseController;
import com.AssignU.controllers.Perfil.PerfilController;
import com.AssignU.models.Clases.ClaseDTO;
import com.AssignU.models.Usuarios.Sesion;
import com.AssignU.utils.ApiCliente;
import com.AssignU.utils.VentanaEmergente;
import javafx.event.ActionEvent;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.Alert;
import javafx.scene.control.Button;
import javafx.scene.input.MouseEvent;
import javafx.scene.layout.FlowPane;
import javafx.scene.layout.StackPane;
import javafx.stage.Modality;
import javafx.stage.Stage;

import java.io.IOException;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class MenuController {
    public FlowPane fpContenedorClases;
    public Button btnAccionClase;
    private Sesion sesion;
    private List<ClaseDTO> listaClases;
    private Map<String, String> headers = new HashMap<String, String>();

    public void cargarValores(Sesion sesion) {
        this.sesion = sesion;

        cargarClases();
        desplegarClases();
    }

    private void cargarClases() {
        try {
            headers.put("Content-Type", "application/json");
            headers.put("Authorization", "Bearer " + sesion.jwt);
            if (sesion.tipoUsuario.matches("alumno")) {
                listaClases = ApiCliente.enviarSolicitudLista("/clases/alumnos/" + sesion.idUsuario + "/clases", "GET", null, headers, ClaseDTO.class);
                btnAccionClase.setText("+ Unirme a clase");
            } else if (sesion.tipoUsuario.matches("docente")) {
                listaClases = ApiCliente.enviarSolicitudLista("/clases/docentes/" + sesion.idUsuario + "/clases", "GET", null, headers, ClaseDTO.class);
                btnAccionClase.setText("+ Crear clase");
            }
        } catch (Exception e) {
            VentanaEmergente.mostrarVentana("Error", null, e.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }
    }

    private void desplegarClases() {
        try {
            fpContenedorClases.getChildren().clear();
            for (ClaseDTO clase : listaClases) {
                FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/Menu/tarjetaClase.fxml"));
                StackPane tarjeta = loader.load();
                TarjetaClaseController controller = loader.getController();
                controller.cargarDatos(clase.getNombreClase(), "Prueba", clase.getIdClase());

                fpContenedorClases.getChildren().add(tarjeta);
            }
        } catch (Exception ex) {
            System.out.println("Error: " + ex.getMessage());
        }
    }

    public void btnLbCerrarSesion(MouseEvent mouseEvent) {
        try {
            this.sesion = null;
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/login.fxml"));
            Parent nuevaVista = loader.load();
            Stage stage = (Stage) btnAccionClase.getScene().getWindow();
            Scene nuevaEscena = new Scene(nuevaVista);
            stage.setScene(nuevaEscena);
        } catch (IOException ex) {
            VentanaEmergente.mostrarVentana("Error al cambiar la vista", null, ex.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }
    }

    public void btnLbPerfil(MouseEvent mouseEvent) {
        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/Perfil/perfil.fxml"));
            Parent vistaPerfil = loader.load();
            
            PerfilController controller = loader.getController();
            controller.cargarValores(sesion);
            
            Stage escenario = (Stage) btnAccionClase.getScene().getWindow();
            Scene nuevaEscena = new Scene(vistaPerfil);
            escenario.setScene(nuevaEscena);
        } catch (Exception ex) {
            VentanaEmergente.mostrarVentana("Error al cambiar la vista", null, ex.getMessage(), Alert.AlertType.ERROR).showAndWait();
        }
    }

    public void clicBtnAccionClase(ActionEvent actionEvent) {
        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/Clase/crearClase.fxml"));
            Parent nuevaVista = loader.load();
            CrearUnirseAClaseController controller = loader.getController();
            controller.cargarValores(this, sesion);
            Stage nuevaVentana = new Stage();
            nuevaVentana.setScene(new Scene(nuevaVista));
            nuevaVentana.initModality(Modality.APPLICATION_MODAL);
            nuevaVentana.showAndWait();
        } catch (Exception e) {
            System.out.println("Error:" + e.getMessage());
        }
    }

    public void enviarAClaseNueva(ClaseDTO claseDto){
        VentanaEmergente.mostrarVentana("¡BIEN!", null, "La clase " + claseDto.getNombreClase() + " fue creada.", Alert.AlertType.INFORMATION).showAndWait();
    }
}
