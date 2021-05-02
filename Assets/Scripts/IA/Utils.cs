using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.IA
{
    class Utils
    {
		public static float degreeToRad = 0.01745329252f;
		public static bool GameObjectInView(GameObject target, GameObject observer, float fieldOfView = 45f, float viewDistance = 10f, string targetTag = "Player")
		{

			if (target is null) return false;

			RaycastHit hit;

			var distanceRay = target.transform.position - observer.transform.position;
					
			if (distanceRay.magnitude > viewDistance)
				return false;

			// Ignorar coisas que o target pode carregar
			LayerMask layerMask = ~LayerMask.GetMask("Grabbable", "Key");

			if (!Physics.Raycast(observer.transform.position, // Origin
					distanceRay.normalized, // Direction
					out hit, // Escrever os valores na variavel hit
					15f, // Distancia
					layerMask) // Aplicar a mascara descrita acima
				) return false;  // Nao ta vendo nada

			if (!hit.collider.CompareTag(targetTag)) return false;

			if (Vector3.Angle(
				observer.transform.forward,
				distanceRay) > fieldOfView)
				return false;



			return true;

		}

    }
}
