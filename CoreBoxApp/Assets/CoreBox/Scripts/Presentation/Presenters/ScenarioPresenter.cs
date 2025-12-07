using RestBuilder;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CoreBox
{
    internal sealed class ScenarioPresenter : MonoBehaviour
    {
        private IApiService _apiService;
        private ITokenProvider _tokenProvider;

        [SerializeField] private GameObject scenarioPrefab;
        [SerializeField] private Transform scenarioContent;

        [Inject]
        private void Construct(IApiService apiService, ITokenProvider tokenProvider)
        {
            _apiService = apiService;
            _tokenProvider = tokenProvider;
        }

        public async void GetScenarios()
        {
            var result = await _apiService.GetAsync<List<ModuleDto>>("api/Modules/GetAll", new()
            {
                Headers = new()
                {
                    { "Content-Type", "application/json" },
                    { "Authorization", $"Bearer {_tokenProvider.Token}" }
                }
            });

            foreach (var moduleDto in result)
            {
                var inst = Instantiate(scenarioPrefab, scenarioContent).GetComponent<ScenarioViewCard>();
                inst.text.text = moduleDto.Name;
                inst.image.sprite = BytesToSprite(await _apiService.GetBytesAsync(moduleDto.ImageUrl), 90, 90);
            }
        }

        public Sprite BytesToSprite(byte[] bytes, int width, int height)
        {
            Texture2D texture = new Texture2D(width, height);

            texture.LoadImage(bytes);

            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );

            return sprite;
        }
    }

    [Serializable]
    public sealed class ModuleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string BundleUrl { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string Version { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}