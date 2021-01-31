using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ColorPicker : MonoBehaviour,IPointerClickHandler,IPointerDownHandler,IDragHandler
{
    Color[] dataRGB;
    Color currentColor;
    public Color pickedColor;
    Image spRenderer;
    Texture2D texture;

    int height { get { return texture.height; } }
    int width { get { return texture.width; } }

    public SpriteRenderer SpriteToPaint;
    Bluetooth bluetooth;
    GameManager gameManager;

    public CurrentRGBValue red, blue, green;
    public Slider brightnessSlider;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        bluetooth = FindObjectOfType<Bluetooth>();
        //renderer to read the color
        spRenderer = GetComponent<Image>();
        texture = spRenderer.sprite.texture;
        dataRGB = texture.GetPixels();

        brightnessSlider.maxValue = 255;
        brightnessSlider.minValue = 0;
        brightnessSlider.value = 255;

    }
    private void Start()
    {
        brightnessSlider.onValueChanged.AddListener(delegate { bluetooth.WriteBrightness((int)brightnessSlider.value); });
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        GetPixelColor(eventData);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        GetPixelColor(eventData);
    }
    public void OnDrag(PointerEventData eventData)
    {
        GetPixelColor(eventData);
    }
    /// <summary>
    /// Gets the pixel color of the image and stores it into the first color palete
    /// </summary>
    /// <param name="eventData"></param>
    void GetPixelColor(PointerEventData eventData)
    {
        Vector2 screenPos = eventData.position;
        screenPos = new Vector2(screenPos.x, screenPos.y);

        if (eventData.pointerCurrentRaycast.gameObject.tag == "ColorPicker")
        {
            Vector2 localCursor;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localCursor))
                return;

            // Debug.Log("LocalCursor:" + localCursor);
            int x = Mathf.FloorToInt(localCursor.x);
            int y = Mathf.FloorToInt(localCursor.y);

            //Invert the coordinates, textures go all the way arround
            if (x < 0){
                x = x + (int)width / 2;
            }else{
                x += (int)width / 2;
            }
            if (y > 0){
                y = y + (int)height / 2;
            }else{
                y += (int)height / 2;
            }

            if (x > 0 && x < width && y > 0 && y < height)
                if (dataRGB[y * width + x].a == 1)//check if the mouse press is not in the corner of the image where is no color
                {
                    pickedColor = dataRGB[y * width + x];
                    gameManager.SetColorPalete(pickedColor);
                    ///print("red:" + pickedColor.r + " green:" + pickedColor.g + " blue:" + pickedColor.b);
                    SetRGBvalue(pickedColor);
                    SpriteToPaint.color = pickedColor;
                }
        }
        else { return; }
    }

    void SetRGBvalue(Color _color)
    {
        red.OnValueChanged(_color.r *  255);
        green.OnValueChanged(_color.g * 255);
        blue.OnValueChanged(_color.b * 255);
    }
}
