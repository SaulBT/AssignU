package com.AssignU.controllers.Menu;

import com.AssignU.controllers.Clase.CrearUnirseAClaseController;
import com.AssignU.controllers.Perfil.PerfilController;
import com.AssignU.models.Clases.ClaseDTO;
import com.AssignU.models.Usuarios.Sesion;
import com.AssignU.servicios.ServicioClases;
import com.AssignU.utils.ApiCliente;
import com.AssignU.utils.Constantes;
import com.AssignU.utils.Navegador;
import com.AssignU.utils.Utils;
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
import javafx.fxml.FXML;

public class MenuController {
    public FlowPane fpContenedorClases;
    public Button btnAccionClase;
    private Sesion sesion;
    private List<ClaseDTO> listaClases;
    private Map<String, String> headers = new HashMap<String, String>();

    public void cargarValores() {
        this.sesion = Sesion.getSesion();
        cargarClases();
        desplegarClases();
    }

    private void cargarClases() {
        HashMap<String, Object> respuesta = ServicioClases.obtenerClasesDeUsuario(sesion, esDocente);

        if (!(boolean) respuesta.get(Constantes.KEY_ERROR)) {
            @SuppressWarnings("unchecked")
            List<ClaseDTO> listaClases = (List<ClaseDTO>) respuesta.get(Constantes.KEY_RESPUESTA);
            if (esDocente) {
                btnAccionClase.setText("+ Crear clase");
            } else {
                btnAccionClase.setText("+ Unirme a clase");
            }
            // Aquí puedes llenar la vista con listaClases

        } else {
            Utils.mostrarVentana("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
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

    @FXML
    public void btnLbCerrarSesion(MouseEvent mouseEvent) {
        this.sesion = null;

        Navegador.cambiarVentana(
            btnAccionClase.getScene(),
            "/views/login.fxml",
            null
        );
    }

    @FXML
    public void btnLbPerfil(MouseEvent mouseEvent) {
        Navegador.cambiarVentana(
            btnAccionClase.getScene(),
            "/views/Perfil/perfil.fxml",
            controller -> ((PerfilController) controller).cargarValores(sesion)
        );
    }

    @FXML
    public void clicBtnAccionClase(ActionEvent actionEvent) {
        String tituloVentana;
        if(esDocente){
            tituloVentana = "Crear Clase";
        }else{
            tituloVentana = "Unirse a Clase";
        }
        Navegador.abrirVentanaModal(
            "/views/Clase/crearUnirseAClase.fxml",
            tituloVentana,
            controller -> ((CrearUnirseAClaseController) controller).cargarValores(this, sesion, esDocente)
        );
    }

    public void enviarAClaseNueva(ClaseDTO claseDto){
        VentanaEmergente.mostrarVentana("¡BIEN!", null, "La clase " + claseDto.getNombreClase() + " fue creada.", Alert.AlertType.INFORMATION).showAndWait();
    }
}
