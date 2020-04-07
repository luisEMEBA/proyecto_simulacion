using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class viento : MonoBehaviour
{
    int condicional = 500; //Éste valor hace parte de la función que determina cuándo se genera o no viento
    //a futuro la idea es organizar un marco más complejo que permita la generación o no generación de viento
    //dependiendo de variables introducidas por el usuario, por ejemplo intensidad del clima.
    int control_de_particulas = 0;//Variable de control para alternar entre la generación, el movimiento y
    //la eliminación de las partículas de viento.
    int i;//Número que dicta el número de partículas de viento, una vez más puede venir determinado por 
    //variables introducidas por el usuario como intensidad del clima.
    float seno = 1;//Su única función es alternar el valor Y de la posición de las partículas de aire
    //con el fin de emular el movimiento sinusoidal de una corriente de aire.
    float control_seno;//Permite variar la amplitud de los senos descritos por el movimiento de la corriente de aire.
    public List<GameObject> objetos = new List<GameObject>();//Lista que contiene a las partículas generadas.
    Vector3 posicion = new Vector3(0,0,0);//Utilizado para mover las partículas creadas anteriormente.
    float movimiento = 1;//Cantidad de desplazamiento de las partículas de aire, puede parametrizarse bajo variables
    //introducidas por el usuario, ejemplo: Velocidad del viento.
    GameObject g;//Objeto mediante el cuál se instancian las partículas de aire.
    public GameObject bola;//forma física de las partículas de aire
    public GameObject dron;//posición del dron: se utiliza para eliminar las partículas de aire una vez que estas se encuentran 
    //a determinada lejanía del mismo.
    Vector3 posicion_dron;//posición para llevar a cabo lo anterior.
    Vector3 posicion_ultima_particula;//Al restarle a esta posición la del dron obtenemos el parámetro de lejanía específicado anteriormente.
    Vector3 distancia_de_influencia = new Vector3(1,1,1);//resultande de la resta entre posición última partícula y posición dron.
    Vector3 direccion;//Pensada para dotar de dirección y sentido al viento, con lo que sería posible generar corrientes
    //de viento que viajen en varias direcciones, creando un entorno más real.
   void Start(){
       i = 15;//numero de partículas de aire a generar
    }
   void Update(){
    //empieza definición de variables para describir un seno con el movimiento
    control_seno = (Random.Range(0.0f,1.0f));
    seno = seno + 0.1f; 
    if(seno >= 100){
    seno = 0;
    }
    //finaliza definición de variables para describir un seno.
    posicion_dron = dron.gameObject.GetComponent<Transform>().position;
    //Empieza generación de partículas (en caso de cumplir la condición) -- Hay que establecer un marco más complejo para
    //que se cumpla con la misma.    
    condicional = Random.Range(-5000, 5000);
    if(condicional >= -60 && condicional <= 60 && control_de_particulas == 0){
    for (int n = 0; n <= i; n++)
        {
            Vector3 position = new Vector3(n, Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f)); //posición con parámetros aleatorios 
            g = Instantiate(bola, position, Quaternion.identity);//se instancia una partícula de aire.
            objetos.Add(g);//se añade la instancia generada anteriormente a la lista de instancias
            direccion = new Vector3((Random.Range(-1,1)),(Random.Range(-1,1)),(Random.Range(-1,1)));//Se aspiró a dotar una dirección única
            //a cada grupo de partículas generadas
        }    
        control_de_particulas = 1;//es posible avanzar a fase de movimiento
    }
    //finaliza creación de partículas de aire
    //empieza movimiento aplicado a partículas de aire generadas
    if(control_de_particulas == 1){
        posicion.x = 0;
        posicion.y = 0;
        posicion.z = 0; 
        posicion.x = posicion.x + movimiento;
        posicion.z = posicion.z + movimiento;
        posicion.y = (posicion.y + movimiento)*(Mathf.Sin(seno)*control_seno);//para dibujar un seno en el plano xy
        for(int n = 0; n <= i; n++){
        objetos[n].transform.position += posicion;//se actualiza posición
        }
    posicion_ultima_particula = objetos[13].gameObject.GetComponent<Transform>().position;
    distancia_de_influencia = posicion_ultima_particula - posicion_dron;//se empieza a calcular la lejanía de las partículas
    //con respecto al dron, se toma la posición de la penúltima partícula generada.
    }
    //finaliza movimiento aplicado a partículas
    //empieza eliminación de partículas de aire una vez se encuentran suficientemente lejos del dron
    if(distancia_de_influencia.magnitude >= 150){
    foreach (GameObject g in objetos){
     GameObject.Destroy(g);
    }
    objetos.Clear();
       control_de_particulas = 0;//se regresa a fase de generación
    }
    //finaliza eliminación de partículas de aire lejanas.
    }

}
