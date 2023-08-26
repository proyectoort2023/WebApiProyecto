using BDTorneus;
using DTOs_Compartidos.Models;
using FixtureNegocio;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using Negocio.DTOs;
using System.Drawing.Text;
using System.Text.RegularExpressions;
using TorneusClienteWeb.Servicios_de_Datos;
using Utilidades;

namespace TorneusClienteWeb.Servicios
{
    public class FixtureServicio
    {
        private readonly TorneoServicioDatos _torneoServiceDatos;
        private readonly FixtureServicioDatos _fixtureServicioDatos;

        List<PartidoDTO> Partidos = new List<PartidoDTO>();
        List<TablaPosicion> TablaPosiciones = new List<TablaPosicion>();

        public event Action OnActualizarPartidosEvent;
        public int TiempoPromedioMinutos = 0;
        [Inject] private TorneoServicio _torneoServicio { get; set; }
        [Inject] private HubConnection _hubConnection { get; set; }


        public FixtureServicio(TorneoServicio torneoServicio, TorneoServicioDatos torneoServiceDatos, FixtureServicioDatos fixtureServicioDatos, HubConnection hubConnection)
        {
            _torneoServicio = torneoServicio;
            _torneoServiceDatos = torneoServiceDatos;
            _fixtureServicioDatos = fixtureServicioDatos;
            _hubConnection = hubConnection;
        }

        public List<PartidoDTO> ObtenerPartidos()
        {
            return Partidos;
        }

        public async Task SetPartidos(PartidoDTO partido)
        {
            int indiceBuscado = BuscarIndicePartido(partido.Id);
            Partidos[indiceBuscado] = partido;
            //  ver si es necesatio un statehaschanged
        }

        public void SetPartidossTodos(List<PartidoDTO> partidos)
        {
            Partidos = partidos;
        }

        private int BuscarIndicePartido(int partidoId)
        {
            return Partidos.FindIndex(f => f.Id == partidoId);
        }


            public async Task<bool> ArmarFixtureGrupoEliminacionDirecta(List<SelectEquipo> selectEquipos, int cantidadGrupos)
        {
            try
            {
                if (VerificarMinimoPorGrupo(selectEquipos)) throw new Exception("Cada grupo debe tener un minimo de 3 equipos");


                FixtureGrupos fixture = new();
                Partidos = new List<PartidoDTO>();

                FixtureEliminacionDirecta fixtureElimDirecta = new();

                List<GrupoEquipos> grupoequipos = selectEquipos.GroupBy(g => g.Grupo)
                                                         .Select(s => new GrupoEquipos()
                                                         {
                                                             Grupo = s.Key,
                                                             Equipos = s.Select(eq => eq.Equipo).ToList()
                                                         }).ToList();

                Partidos = fixture.Crear(grupoequipos);

                int cantidadEquiposUltimaFase = CantidadEquiposUltimaFase(cantidadGrupos);


                List<PartidoDTO> partidosUltimaFase = fixtureElimDirecta.CrearComoSegundaFase(cantidadEquiposUltimaFase);

                Partidos.AddRange(partidosUltimaFase);

                Partidos.ForEach(o => o.Fecha = _torneoServicio.ObtenerTorneoActual().Fecha);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }

        public async Task<bool> ArmarFixtureTodosContraTodos(List<EquipoDTO> equipos)
        {
            try
            {
                if (equipos.Count < 3) throw new Exception("Debe tener un minimo de 3 equipos");

                Partidos = new List<PartidoDTO>();

                FixtureGrupos fixture = new();

                FixtureEliminacionDirecta fixtureElimDirecta = new();

                List<GrupoEquipos> grupoequipos = new();

                GrupoEquipos grupoEquipos = new()
                {
                    Grupo = "A",
                    Equipos = equipos
                };

                grupoequipos.Add(grupoEquipos);

                Partidos = fixture.Crear(grupoequipos);

                int cantidadEquiposUltimaFase = CantidadEquiposUltimaFase(1);


                List<PartidoDTO> partidosUltimaFase = fixtureElimDirecta.CrearComoSegundaFase(cantidadEquiposUltimaFase);

                Partidos.AddRange(partidosUltimaFase);

                Partidos.ForEach(o => o.Fecha = _torneoServicio.ObtenerTorneoActual().Fecha);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<bool> ArmarFixtureEliminacionDirecta(List<EquipoDTO> equipos)
        {
            try
            {
                if (equipos.Count < 2) throw new Exception("Debe tener un minimo de 2 equipos");

                Partidos = new List<PartidoDTO>();

                FixtureEliminacionDirecta fixtureElimDirecta = new();

                Partidos = fixtureElimDirecta.Crear(equipos);
                Partidos.ForEach(o => o.Fecha = _torneoServicio.ObtenerTorneoActual().Fecha);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private bool VerificarMinimoPorGrupo(List<SelectEquipo> selectEquipos)
        {
            Dictionary<string, int> equiposPorGrupo = new Dictionary<string, int>();

            foreach (var equipo in selectEquipos)
            {
                if (equiposPorGrupo.ContainsKey(equipo.Grupo))
                {
                    equiposPorGrupo[equipo.Grupo]++;
                }
                else
                {
                    equiposPorGrupo.Add(equipo.Grupo, 1);
                }
            }

            // Verificar si algún grupo tiene menos de 3 equipos
            bool hayGrupoConMenosDe3Equipos = equiposPorGrupo.Any(grupo => grupo.Value < 3);

            return hayGrupoConMenosDe3Equipos;

        }

        private int CantidadEquiposUltimaFase(int cantidad)
        {
            double valor = 1;
            int incrementador = 0;

            while (valor < cantidad)
            {
                incrementador++;
                valor = Math.Pow(2, incrementador);
            }
            if (cantidad == 1)
            {
                valor = 2;
            }

            return (int)valor;
        }


        public string ObtenerTiempoJornada()
        {
            var torneoActual = _torneoServicio.ObtenerTorneoActual();

            int cantidadCanchas = torneoActual.CantidadCanchas;
            int cantidadSetsPorPartido = torneoActual.SetsMax;
            int puntajePorSet = torneoActual.PuntajeMax;
            int cantidadPartidos = Partidos.Count();

            double minutosPorSet = 20 * puntajePorSet / 25;
            double minutosPorPartido = minutosPorSet * cantidadSetsPorPartido;
            double jornada = minutosPorPartido * cantidadPartidos / cantidadCanchas; //minutos

            // Calcular las horas y minutos
            int horas = (int)(jornada / 60); 
            int minutosRestantes = (int)(jornada % 60);

            return $"{horas} hora/s y {minutosRestantes} minutos";
        }


        public async Task<bool> GuardarFixture()
        {
            try
            {
                PartidosTorneo partidoTorneo = new()
                {
                    TorneoId = _torneoServicio.ObtenerTorneoActual().Id,
                    Fixture = Partidos
                };
                List<PartidoDTO> partidosCreados = await _fixtureServicioDatos.CrearFixtureTorneo(partidoTorneo);
                Partidos = partidosCreados;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        #region Conteo de Puntaje de marcador
        public async Task<bool> ObtenerFixtureTorneoDatos()
        {
            try
            {
                int torneoId = _torneoServicio.ObtenerTorneoActual().Id;
                List<PartidoDTO> partidosObtenidos = await _fixtureServicioDatos.ListadoPartidosTorneo(torneoId);
                Partidos = partidosObtenidos;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task CrearTablaPosiciones()
        {
            TablaPosiciones = new List<TablaPosicion>();
            var filtroEquipos = Partidos.Where(w => !string.IsNullOrEmpty(w.Grupo))
                                        .SelectMany(p => new[] {
                                            
                                            new {Equipo = p.EquipoLocal, Grupo = p.Grupo },
                                            new {Equipo = p.EquipoVisitante, Grupo = p.Grupo } 
                                        
                                        })
                                        .GroupBy(e => e.Equipo.Id)
                                        .Select(g => g.First())
                                        .ToList();


            TablaPosiciones = filtroEquipos.Select(equipo => new TablaPosicion(equipo.Equipo, equipo.Grupo)).ToList();
        }

        public bool HayCanchaLibre()
        {
            int cantidadCanchas = _torneoServicio.ObtenerTorneoActual().CantidadCanchas;
            int partidosEnProceso = Partidos.Where(w => w.EstadoPartido == Util.EstadoPartido.EN_PROCESO.ToString()).Count();

            return !(partidosEnProceso == cantidadCanchas);
        }

        public async Task<List<PartidoDTO>> TerminoPartido(PartidoDTO partidoFinalizado)
        {
            List<PartidoDTO> partidosActualizar = new();
            

          if (!string.IsNullOrEmpty(partidoFinalizado.Grupo))
            {
                bool etapaGruposFinalizada = EtapaGrupoEstaFinalizada();

                if (etapaGruposFinalizada)
                {
                    partidosActualizar = await SiguienteEtapa();
                }
            }
            else
            {
                partidosActualizar = await DesignarEquipoASiguientePartido(partidoFinalizado);
            }
            return partidosActualizar;
        }

        private async Task<List<PartidoDTO>> DesignarEquipoASiguientePartido(PartidoDTO partidoFinalizado)
        {

            List<PartidoDTO> partidosActualizar = new();

            EquipoDTO equipoGanador = partidoFinalizado.PuntajeLocal > partidoFinalizado.PuntajeVisitante ? partidoFinalizado.EquipoLocal : 
                                                                                                            partidoFinalizado.EquipoVisitante;


            if (partidoFinalizado.PartidoSigGanador != Guid.Empty)
            {
                int indiceSigPartido = ObtenerIndiceGuidPartido(partidoFinalizado.PartidoSigGanador);
                if (Partidos[indiceSigPartido] != null)
                {
                    if (Partidos[indiceSigPartido].EquipoLocal.Id == 0)
                    {
                        Partidos[indiceSigPartido].EquipoLocal = equipoGanador;
                    }
                    else if (Partidos[indiceSigPartido].EquipoVisitante.Id == 0)
                    {
                        Partidos[indiceSigPartido].EquipoVisitante = equipoGanador;
                    }
                }
                partidosActualizar.Add(Partidos[indiceSigPartido]);
            }

            return partidosActualizar;
            //actualizar los partidos siguientes a donde hacen referencia y actualizar en un evento general 
            //   if (partidoFinalizado.PartidoSigPerdedor != Guid.Empty)
            //{
            //    //actualizo si es doble eliminacion
            //}
        }

        private int ObtenerIndiceGuidPartido(Guid guid)
        {
            return Partidos.FindIndex(f => f.GuidPartido == guid);
        }

        private bool EtapaGrupoEstaFinalizada()
        {
            bool partidosGruposNOEstanFinalizados = Partidos.Where(w => !string.IsNullOrEmpty(w.Grupo))
                                                                                        .Any(a => a.EstadoPartido != Util.EstadoPartido.FINALIZADO.ToString());
            return !partidosGruposNOEstanFinalizados;
        }

        private async Task ActualizarTablaPosiciones(List<PartidoDTO> partidos)
        {
            await CrearTablaPosiciones();

            foreach(var partido in partidos)
            {
                EquipoDTO equipo1 = partido.EquipoLocal;
                EquipoDTO equipo2 = partido.EquipoVisitante;

                int indiceLocal = TablaPosiciones.FindIndex(f => f.Equipo.Id == equipo1.Id);
                int indiceVisitante = TablaPosiciones.FindIndex(f => f.Equipo.Id == equipo2.Id);

                var HistorialSetsLista = ConvertirStringALista(partido.HistorialSet);
                int SumaPuntajeLocal = HistorialSetsLista.Sum(w => w.Local);
                int SumaPuntajeVisitante = HistorialSetsLista.Sum(w => w.Visitante);

                TablaPosiciones[indiceLocal].ActualizarTabla(partido.SetGanadosLocal, partido.PuntajeLocal, partido.Inicio, partido.Fin, SumaPuntajeLocal);
                TablaPosiciones[indiceVisitante].ActualizarTabla(partido.SetGanadosVisitante, partido.PuntajeVisitante, partido.Inicio, partido.Fin, SumaPuntajeVisitante);
            }
        }

        public List<ResultadoMatchesPuntaje> ConvertirStringALista(string convetir)
        {
            List<ResultadoMatchesPuntaje> lista = new();

            var matches = Regex.Matches(convetir, @"\[(\d+)-(\d+)\]");

            foreach (Match match in matches)
            {
                ResultadoMatchesPuntaje res = new()
                {
                    Local = int.Parse(match.Groups[1].Value),
                    Visitante = int.Parse(match.Groups[2].Value)
                };

                lista.Add(res);
            }
            return lista;
        }

        private async Task<List<PartidoDTO>> SiguienteEtapa()
        {
            List<EquipoDTO> equiposSegundaFase = new List<EquipoDTO>();
            List<PartidoDTO> partidosActualizar = new List<PartidoDTO>();

            var partidosGrupos = Partidos.Where(w => !string.IsNullOrEmpty(w.Grupo)).ToList();

            await ActualizarTablaPosiciones(partidosGrupos);

            int cantidadGrupos = TablaPosiciones.Select(s => s.Grupo).Distinct().Count();

            int cantidadEquiposAJugar = CantidadEquiposUltimaFase(cantidadGrupos);

            int cantidadEquiposSiguientesMejores = cantidadEquiposAJugar - cantidadGrupos;

            var tablaPosicionesView = ObtenerTablaPosicionesView();

            for (int i = 0; i< cantidadGrupos; i++)
            {
                equiposSegundaFase.Add(tablaPosicionesView[i].TablaPosiciones.First().Equipo); //primeros de cada grupo
            }

            if (cantidadEquiposSiguientesMejores > 0)
            {
                var equiposSegundosMejores = ObtenerEquiposSegundosMejoresView(tablaPosicionesView);

                for (int i = 0; i < cantidadEquiposSiguientesMejores; i++)
                {
                    equiposSegundaFase.Add(equiposSegundosMejores[i]);
                }

            }
            //Asignar equipos a partidos siguientes;

            int cantidadPartidosJugarInicial = cantidadEquiposAJugar / 2;

            List<PartidoDTO> partidosElimDirecta = Partidos.Where(w => w.EstadoPartido != Util.EstadoPartido.FINALIZADO.ToString()).ToList();

            if (cantidadEquiposAJugar == equiposSegundaFase.Count)
            {
                for (int i = 0; i < cantidadEquiposAJugar; i+=2)
                {
                    //int indicePartido = 0;
                    //for (int j = 0; j < cantidadPartidosJugarInicial; j++)
                    //{
                       int indicePartido = ObtenerIndicePartido(partidosElimDirecta[i/2].Id);
                    //}
                    Partidos[indicePartido].EquipoLocal = equiposSegundaFase[i];
                    Partidos[indicePartido].EquipoVisitante = equiposSegundaFase[i+1];
                    partidosActualizar.Add(Partidos[indicePartido]);
                }
            }
            return partidosActualizar;
        }

        private int ObtenerIndicePartido(int partidoId)
        {
            return Partidos.FindIndex(f => f.Id == partidoId);
        }

        public async Task<List<GrupoTablaPosicionesView>> ObtenerTablaPosiciones()
        {
            var partidosGrupos = Partidos.Where(w => !string.IsNullOrEmpty(w.Grupo) && w.EstadoPartido == Util.EstadoPartido.FINALIZADO.ToString()).ToList();

            await ActualizarTablaPosiciones(partidosGrupos);

            return ObtenerTablaPosicionesView();
        }

        private List<GrupoTablaPosicionesView> ObtenerTablaPosicionesView()
        {


            List<GrupoTablaPosicionesView> tablaOrdenadaView = TablaPosiciones.GroupBy(g => g.Grupo)
                                                                                .Select(s => new GrupoTablaPosicionesView()
                                                                                {
                                                                                    Grupo = s.Key,
                                                                                    TablaPosiciones = s.OrderByDescending(x => x.PartidosGanados)
                                                                                                       .ThenByDescending(x => x.SetsGanados)
                                                                                                       .ThenByDescending(x => x.TotalPuntos)
                                                                                                       .ThenByDescending(x => x.MejorTiempo)
                                                                                                       .ThenByDescending(x => x.PartidosJugados)
                                                                                                       .ToList()
                                                                                })
                                                                                .OrderBy(o => o.Grupo)
                                                                                .ToList();
            return tablaOrdenadaView;
        }
        public List<EquipoDTO> ObtenerEquiposSegundosMejoresView(List<GrupoTablaPosicionesView> tablaOrdenadaView)
        {
            var equiposSegundosMejores = tablaOrdenadaView.Select(s => s.TablaPosiciones[1])
                                                           .OrderByDescending(x => x.PartidosGanados)
                                                           .ThenByDescending(x => x.SetsGanados)
                                                           .ThenByDescending(x => x.TotalPuntos)
                                                           .ThenByDescending(x => x.MejorTiempo)
                                                           .ThenByDescending(x => x.PartidosJugados)
                                                           .Select(s => s.Equipo)
                                                           .ToList();


            return equiposSegundosMejores;
        }

        public async Task ActualizarListadoPartidosFront()
        {
            OnActualizarPartidosEvent?.Invoke();
        }

        #endregion

        public async Task ActualizacionPartidosTiempoReal(List<PartidoDTO> partidos)
        {
            await _hubConnection.SendAsync("EnviarActualizarPartidos", partidos);
        }

        //public bool EsFinalTorneo()
        //{
        //    return !Partidos.Any(a => a.EstadoPartido != Util.EstadoPartido.FINALIZADO.ToString());
        //}
        
        public async Task<bool> ActualizarPartidoTorneo(PartidoDTO partido)
        {
            try
            {
                return await _fixtureServicioDatos.ActualizarPartido(partido);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool HayEtapaGrupos()
        {
            return Partidos.Any(a => !string.IsNullOrEmpty(a.Grupo));
        }

        public async Task ActualizarTiempoPromedioPartidos()
        {
            if (Partidos != null)
            {
                if (Partidos.Count > 0)
                {
                    var partidosFinalizados = Partidos.Where(w => w.EstadoPartido == Util.EstadoPartido.FINALIZADO.ToString()).ToList();
                    int cantidadPartidosFinalizados = partidosFinalizados.Count;

                    int promedio = 0;

                    foreach (var partido in partidosFinalizados)
                    {
                        promedio += Util.TiempoEnMinutos(partido.Inicio, partido.Fin);
                    }
                    TiempoPromedioMinutos = cantidadPartidosFinalizados > 0 ? (int)(promedio / cantidadPartidosFinalizados) : 0;
                }
            }
        }








    }
}
