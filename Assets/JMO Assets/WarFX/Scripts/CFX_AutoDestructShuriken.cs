using UnityEngine;
using System.Collections;
using Unity.Netcode;

[RequireComponent(typeof(ParticleSystem))]
public class CFX_AutoDestructShuriken : NetworkBehaviour
{
	public bool OnlyDeactivate;
	
	void OnEnable()
	{
		StartCoroutine("CheckIfAlive");
	}
	[ServerRpc (RequireOwnership =false)]
	public void DestoryProjectileServerRpc() {
		GetComponent<NetworkObject>().Despawn();
		GameObject.Destroy(this.gameObject);
	}
	IEnumerator CheckIfAlive ()
	{
		while(true)
		{
			yield return new WaitForSeconds(0.5f);
			if(!GetComponent<ParticleSystem>().IsAlive(true))
			{
				if (OnlyDeactivate)
				{
#if UNITY_3_5
						this.gameObject.SetActiveRecursively(false);
#else
					this.gameObject.SetActive(false);
#endif
				}
				else
				{

					DestoryProjectileServerRpc();

				}
				break;
			}
		}
	}
}
