using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EncyclopediaMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pageCounter;
    [SerializeField] private GameObject firstPage;
    [SerializeField] private GameObject secondPage;
    [SerializeField] private GameObject thirdPage;

    private int currentPage = 1;

    public void OnNextPageClick()
    {
        if(currentPage == 1)
        {
            firstPage.SetActive(false);
            secondPage.SetActive(true);
        }
        if(currentPage == 2)
        {
            secondPage.SetActive(false);
            thirdPage.SetActive(true);
        }
        if (currentPage < 3)
            currentPage++;
        pageCounter.text = currentPage + "/3";
    }

    public void OnPreviousPageClick()
    {
        if (currentPage == 2)
        {
            firstPage.SetActive(true);
            secondPage.SetActive(false);
        }
        if (currentPage == 3)
        {
            secondPage.SetActive(true);
            thirdPage.SetActive(false);
        }
        if (currentPage > 1)
           currentPage--;
        pageCounter.text = currentPage + "/3";
    }
}
