package com.AssignU.controllers.Menu;

import com.AssignU.controllers.Clase.ClaseController;
import com.AssignU.controllers.Clase.CrearUnirseAClaseController;
import com.AssignU.controllers.Perfil.PerfilController;
import com.AssignU.models.Clases.ClaseDTO;
import com.AssignU.models.Usuarios.Sesion;
import com.AssignU.servicios.ServicioClases;
import com.AssignU.servicios.usuarios.ServicioDocentes;
import com.AssignU.utils.Constantes;
import com.AssignU.utils.Navegador;
import com.AssignU.utils.Utils;
import java.io.IOException;
import javafx.event.ActionEvent;
import javafx.fxml.FXMLLoader;
import javafx.scene.control.Alert;
import javafx.scene.control.Button;
import javafx.scene.input.MouseEvent;
import javafx.scene.layout.FlowPane;
import javafx.scene.layout.StackPane;

import java.util.HashMap;
import java.util.List;
import java.util.Map;
import javafx.fxml.FXML;

public class MenuController {
    public FlowPane fpContenedorClases;
    public Button btnAccionClase;
    private Sesion sesion;
    private Map<String, String> headers = new HashMap<String, String>();

    public void cargarValores() {
        this.sesion = Sesion.getSesion();
        cargarClases();
    }

    private void cargarClases() {
        HashMap<String, Object> respuesta = ServicioClases.obtenerClasesDeUsuario();

        if (!(boolean) respuesta.get(Constantes.KEY_ERROR)) {
            List<ClaseDTO> listaClases = (List<ClaseDTO>) respuesta.get(Constantes.KEY_RESPUESTA);
            if (sesion.esDocente()) {
                btnAccionClase.setText("+ Crear clase");
            } else {
                btnAccionClase.setText("+ Unirme a clase");
            }
            desplegarClases(listaClases);
        } else {
            Utils.mostrarVentana("Error", (String) respuesta.get(Constantes.KEY_MENSAJE), Alert.AlertType.ERROR);
        }

    }

    private void desplegarClases(List<ClaseDTO> listaClases) {
        try {
            fpContenedorClases.getChildren().clear();
            for (ClaseDTO clase : listaClases) {
                FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/Menu/tarjetaClase.fxml"));
                StackPane tarjeta = loader.load();
                TarjetaClaseController controller = loader.getController();
                if(sesion.esDocente()){
                    controller.cargarDatos(clase, "Código: " + clase.codigoClase);
                } else {
                    HashMap<String, Object> respuesta = ServicioDocentes.obtenerNombreDocente(clase.idDocente);
                    if (!(boolean) respuesta.get(Constantes.KEY_ERROR)) {
                        controller.cargarDatos(clase, (String) respuesta.get(Constantes.KEY_MENSAJE));
                    } else {
                        Utils.mostrarVentana("Error", 
                                "Ocurrió un error obteniendo al docente: " + (String) respuesta.get(Constantes.KEY_MENSAJE), 
                                Alert.AlertType.ERROR);
                        respuesta.clear();
                        break;
                    }
                    respuesta.clear();
                }
                fpContenedorClases.getChildren().add(tarjeta);
            }
        } catch (IOException ex) {
            System.out.println("Error: " + ex.getMessage());
        }
    }

    @FXML
    public void btnLbCerrarSesion(MouseEvent mouseEvent) {
        Sesion.cerrarSesion();
        Navegador.cambiarVentana(
            btnAccionClase.getScene(),
            "/views/login.fxml",
            "Inicio de Sesión",
            null
        );
    }

    @FXML
    public void btnLbPerfil(MouseEvent mouseEvent) {
        Navegador.cambiarVentana(
            btnAccionClase.getScene(),
            "/views/Perfil/perfil.fxml",
            "Mi Perfil",
            controller -> ((PerfilController) controller).cargarValores()
        );
    }

    @FXML
    public void clicBtnAccionClase(ActionEvent actionEvent) {
        String tituloVentana;
        if(sesion.esDocente()){
            tituloVentana = "Crear Clase";
        }else{
            tituloVentana = "Unirse a Clase";
        }
        Navegador.abrirVentanaModal(
            "/views/Clase/crearUnirseAClase.fxml",
            tituloVentana,
            controller -> ((CrearUnirseAClaseController) controller).cargarValores(this)
        );
    }

    public void enviarAClaseNueva(ClaseDTO claseDto){
        Navegador.cambiarVentana(
            btnAccionClase.getScene(),
            "/views/Clase/clase.fxml",
            claseDto.nombreClase,
            controller -> ((ClaseController) controller).cargarValoresDeMenu(claseDto)
        );
    }
}
