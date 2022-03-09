using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Estado{

  public string nome{ get; set; }
  public string sigla{ get; set; }
  public List<Cidade> cidades { get; set; }

  public Estado(string nome, string sigla){
    if(nome != "" && sigla != "") {
      this.nome = nome;
      this.sigla = sigla;
      this.cidades = new List<Cidade>();
    }
    else throw new Exception("É necessário preencher o nome e a sigla do estado.");
  }

  public Cidade getCapital(){
    foreach(Cidade cidade in this.cidades){
      if(cidade.capital) return cidade;
    }
    return null;
  }

  public override string ToString(){
      return $"Estado: {nome}\nSigla: {sigla}\nNúmeros de cidades: {cidades.Count}";
  }
}

public class Cidade{
  public string nome{ get; set; }
  public bool capital{ get; set; }
  public Estado estado;
  
  public Cidade(string nome, bool capital, Estado estado){
    if(nome != "" && estado != null){
      this.nome = nome;
      this.capital = capital;
      this.estado = estado;
    }
    else throw new Exception("É necessário preencher o nome da cidade e selecionar o estado.");
  }

  public override string ToString(){
    string isCapital = capital ? "Sim" : "Não";
      return $"Nome da cidade: {nome}\nÉ capital: {isCapital}\nEstado da cidade: {estado.nome} ({estado.sigla})";
  }
}

class Sistema {
  private int countEstado = 0;
  private Estado[] estados = new Estado[27];
  private List<Cidade> cidades = new List<Cidade>();

  public int getNumeroEstados(){
    return this.countEstado;
  }

  public void inserirEstado(string nome, string sigla){
    try{
      Estado novoEstado = new Estado(nome, sigla);
      this.estados[this.countEstado++] = novoEstado;
    } catch(Exception error){
      throw error;
    }
  }

  public void inserirCidade(string nome, bool capital, Estado estado){
    try{
      Cidade novaCidade = new Cidade(nome, capital, estado);
      int indexEstado = this.procurarEstado(estado);
      if(indexEstado == -1) 
        throw new Exception("Estado não existe.");
      
      this.cidades.Add(novaCidade);
      this.estados[indexEstado].cidades.Add(novaCidade);
    } catch(Exception error){
      throw error;
    }
  }

  public Estado[] listarEstados(){
    return this.estados;
  }

  public Estado[] listarEstadosSemCapital(){
    Estado[] estadosSemCapital = new Estado[this.countEstado];
    for(int i = 0; i < this.countEstado; i++){
      if(this.estados[i].getCapital() == null) 
        estadosSemCapital[i] = this.estados[i];
    }
    
    foreach(Estado estado in estadosSemCapital) 
      if(estado != null) 
        return estadosSemCapital;
    
    return null;
  }

  public List<Cidade> listarCidades(){
    return this.cidades;
  } 

  public int procurarEstado(Estado estado){
    return Array.FindIndex(this.estados, (e) => (e.nome == estado.nome));
  }

  public Cidade procurarCidade(string cidade){
    foreach(Cidade c in this.cidades){
      if(c.nome.ToUpper() == cidade.ToUpper()) return c;
    }
    return null;
  }
}

class Program {
  
  public static void Main (string[] args) {
    Sistema sistema = new Sistema();
    string entrada;
    Program.getMenu();
    while((entrada = Console.ReadLine()) != "0"){
      setOptionMenu(sistema, entrada);
    }
  }

  public static void getMenu(){
    Console.WriteLine("Escolha uma das opções abaixo:");
    Console.WriteLine("[1] - Inserir estado");
    Console.WriteLine("[2] - Inserir cidade");
    Console.WriteLine("[3] - Listar estados");
    Console.WriteLine("[4] - Listar cidades");
    Console.WriteLine("[5] - Mostrar capital de um estado");
    Console.WriteLine("[6] - Verificar se cidade é capital");
    Console.WriteLine("[0] - Parar execução");
  }

  public static void setOptionMenu(Sistema sistema, string option){
    Console.Clear();
    switch (option)
    {
      case "1":
        Console.WriteLine("Informe o nome do estado:");
        string nomeEstado = Console.ReadLine();
        Console.WriteLine("");
    
        Console.WriteLine("Informe a sigla do estado:");
        string siglaEstado = Console.ReadLine();
        Console.WriteLine("");

        Console.Clear();
        
        try{
          sistema.inserirEstado(nomeEstado, siglaEstado);
          Console.WriteLine("Estado inserido com sucesso!");
        }
        catch(Exception e){
          Console.WriteLine(e.Message+"\n");
        }
      
      break;
      case "2":
        string capital = "";
        bool isCapital = false;
        
        Console.WriteLine("Informe o nome da cidade:");
        string nomeCidade = Console.ReadLine();
        Console.WriteLine("");

        do{
          Console.WriteLine("Está cidade é uma capital (S/N)?");
          capital = Console.ReadLine();
        } while(capital.ToUpper() != "S" && capital.ToUpper() != "N");
        
        isCapital = capital.ToUpper() == "S" ? true : false;
        Console.Clear();
        
        Console.WriteLine("Qual estado está cidade pertence?\n");

        Estado[] estadosCidade;
        if(isCapital){
          estadosCidade = sistema.listarEstadosSemCapital(); 
          
          if(estadosCidade == null){
            Console.Clear();
            Console.WriteLine("Não há estados sem capital no sistema.");
            break;
          }
          
          for(int i = 0; i < estadosCidade.Count(); i++){
            if(estadosCidade[i] != null) Console.WriteLine($"[{i}] - {estadosCidade[i].nome}");
          }
          
        } else {
          estadosCidade = sistema.listarEstados();
          for(int i = 0; i < sistema.getNumeroEstados(); i++){
            Console.WriteLine($"[{i}] - {estadosCidade[i].nome}");
          }
        }

        int index = int.Parse(Console.ReadLine());
        Estado estadoCidade = estadosCidade[index];
        
        if(estadoCidade == null) {
          Console.WriteLine("Estado não existe.\n");
          break;
        }
        
        sistema.inserirCidade(nomeCidade, isCapital, estadoCidade);
        Console.Clear();
        Console.WriteLine("Cidade inserida com sucesso!");
        
      break;
      case "3":
        Estado[] estados = sistema.listarEstados();
        
        if(sistema.getNumeroEstados() == 0) Console.WriteLine("Não tem estados cadastrados.");
        else{
          Console.WriteLine("--- Lista de estados ---\n");
          foreach(Estado e in estados){
            if(e != null) Console.WriteLine($"{e}\n");
          }
          Console.WriteLine("------------------------\n");
        }
    
    
      break;
      case "4":
        List<Cidade> cidades = sistema.listarCidades();
        
        if(cidades.Count == 0) {
          Console.WriteLine("Não há cidades cadastradas.");
          break;
        }
        
        Console.WriteLine("--- Lista de cidades ---\n");
        foreach(Cidade c in cidades){
          if(c != null) 
            Console.WriteLine($"{c}\n");
        }
        Console.WriteLine("------------------------\n");
        
      break;    
      case "5":
        if(sistema.getNumeroEstados() == 0){
          Console.WriteLine("Não tem estados cadastrados.");
          break;
        }
        
        Console.WriteLine("Escolha o estado:\n");
        
        for(int i = 0; i < sistema.getNumeroEstados(); i++){
          Console.WriteLine($"[{i}] - {sistema.listarEstados()[i].nome}");
        }

        try{
          int indexEstado = int.Parse(Console.ReadLine());
          Console.Clear();
          Estado estado = sistema.listarEstados()[indexEstado];
          if(estado.getCapital() != null){
            Console.WriteLine($"A capital do(a) {estado.nome} é {estado.getCapital().nome}\n");
          }
          else Console.WriteLine($"A capital do(a) {estado.nome} não está cadastrada.\n");
        } 
        catch(FormatException){
          Console.WriteLine("Formato de input errado.");
        }
        catch(NullReferenceException){
          Console.WriteLine("Não existe Estado nessa posição.");
        }
        catch(Exception e){
          Console.WriteLine(e.Message);
        }
      break;
      case "6":
        Console.WriteLine("Digite o nome da cidade:");
        string cidadeBusca = Console.ReadLine();
        Console.WriteLine("");
        
        Cidade cidade = sistema.procurarCidade(cidadeBusca);
        
        if(cidade != null){
          Console.WriteLine(cidade);
        }
        else{
          Console.WriteLine("Não conseguimos encontrar a cidade solicitada.");
        }

      break;
      default:
        Console.Clear();
        Console.WriteLine("Input não reconhecido, tente outro valor.\n");
      break;
    }
    Console.WriteLine("\nPressione 'Enter' para continuar.");
    Console.ReadLine();
    Console.Clear();
    Program.getMenu();
  }
}