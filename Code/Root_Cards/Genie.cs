using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ItemShops.Extensions;
using ItemShops.Utils;
using RarityLib.Utils;
using TMPro;
using UnboundLib;
using UnityEngine;

public class Genie 
{
    public static Shop Genie_Shop;
    public static string ShopID = "Root_Genie_Shop";
    public static Dictionary<String, int> wishes = new Dictionary<String, int>();

    internal static IEnumerator Wish()
        {
            wishes = new Dictionary<string, int>();
            wishes.Add("Wish", 1);
            if (Genie_Shop != null) ShopManager.instance.RemoveShop(Genie_Shop); 
            Genie_Shop = ShopManager.instance.CreateShop(ShopID);
            Genie_Shop.UpdateMoneyColumnName("Wishes");
            Genie_Shop.UpdateTitle("Be Carful What You Wish For");
            //GameModeManager.AddOnceHook(GameModeHooks.HookPickStart, (gm) => SetUpShopStart());
            RootCards.instance.StartCoroutine(SetUpShop());
            yield break;
        }

        internal static IEnumerator SetUpShop()
        {
            List<UnboundLib.Utils.Card> allCards = UnboundLib.Utils.CardManager.cards.Values.ToList();
            List<CardItem> items = new List<CardItem>();
            foreach (UnboundLib.Utils.Card card in allCards)
            {
                if (card != null && card.cardInfo != CardResgester.ModCards["Genie"] && UnboundLib.Utils.CardManager.IsCardActive(card.cardInfo)) {
                    items.Add(new CardItem(card));
                }
            }
            Genie_Shop.AddItems(items.Select(c => c.Card.cardInfo.name).ToArray(), items.ToArray());
            yield break;
        }

        internal static IEnumerator WaitTillShopDone()
        {
            bool done = true;
            GameObject gameObject = null;
            GameObject timer = null;
            float time = 120;
            PlayerManager.instance.players.ForEach(p =>
            {
                if (p.GetAdditionalData().bankAccount.HasFunds(wishes)){ Genie_Shop.Show(p); done = false; }
            });

            if (!done)
            {
                gameObject = new GameObject();
                gameObject.AddComponent<Canvas>().sortingLayerName = "MostFront";
                gameObject.AddComponent<TextMeshProUGUI>().text = "Wating For Players In Wish Menu";
                Color c = Color.magenta;
                c.a = .85f;
                gameObject.GetComponent<TextMeshProUGUI>().color = c;
                gameObject.transform.localScale = new Vector3(.2f, .2f);
                gameObject.transform.localPosition = new Vector3(0, 5);
                timer = new GameObject();
                timer.AddComponent<Canvas>().sortingLayerName = "MostFront";
                timer.transform.localScale = new Vector3(.2f, .2f);
                timer.transform.localPosition = new Vector3(0, 16);
                timer.AddComponent<TextMeshProUGUI>().color = c;
                for (int i = 0; i < 5; i++)
                {
                    timer.GetComponent<TextMeshProUGUI>().text = ((int)time).ToString();
                    yield return new WaitForSecondsRealtime(1f);
                    time -= 1;
                }
            }
            while (!done)
            {
                timer.GetComponent<TextMeshProUGUI>().text = ((int)time).ToString();
                done = true;
                yield return new WaitForSecondsRealtime(0.2f);
                time -= 0.2f;
                PlayerManager.instance.players.ForEach(p => 
                {
                    if (ShopManager.instance.PlayerIsInShop(p)) done = false;
                });
                if (time <= 0)
                    ShopManager.instance.HideAllShops();

            }
            GameObject.Destroy(gameObject);
            GameObject.Destroy(timer);
        }

        internal static IEnumerator RestCardLock()
        {
            foreach (Player player in PlayerManager.instance.players.ToArray())
            {
                player.data.stats.GetRootData().lockedCard = null;
            }
            yield break;
        }
}

internal class CardItem : Purchasable
    {
        internal UnboundLib.Utils.Card Card;
        private Dictionary<string, int> cost = new Dictionary<string, int>();
        public CardItem(UnboundLib.Utils.Card card)
        {
            Card = card;
            cost.Add("Wish", 1);
        }
        public override string Name { get { return Card.cardInfo.cardName; } }

        public override Dictionary<string, int> Cost { get{ return cost; } } 

        public override Tag[] Tags { get{ return new Tag[] { new Tag(Card.cardInfo.rarity.ToString()), new Tag(Card.category) }; } } 

        public override bool CanPurchase(Player player)
        {
            return true;
        }

        public override GameObject CreateItem(GameObject parent)
        {
            GameObject container = null;
            GameObject holder = null;

            try
            {
                container = GameObject.Instantiate(ItemShops.ItemShops.instance.assets.LoadAsset<GameObject>("Card Container"));
            }
            catch (Exception)
            {

                UnityEngine.Debug.Log("Issue with creating the card container");
            }

            try
            {
                holder = container.transform.Find("Card Holder").gameObject;
            }
            catch (Exception)
            {

                UnityEngine.Debug.Log("Issue with getting the Card Holder");
                holder = container.transform.GetChild(0).gameObject;
            }
            holder.transform.localPosition = new Vector3(0f, -100f, 0f);
            holder.transform.localScale = new Vector3(0.125f, 0.125f, 1f);
            holder.transform.Rotate(0f, 180f, 0f);

            GameObject cardObj = null;

            try
            {
                cardObj = GetCardVisuals(Card.cardInfo, holder);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log("Issue with getting card visuals");
                UnityEngine.Debug.LogError(e);
            }

            container.transform.SetParent(parent.transform);

            return container;
        }

        public override void OnPurchase(Player player, Purchasable item)
        {
            var card = ((CardItem)item).Card.cardInfo; 
            System.Random r = new System.Random();/*
            switch (card.rarity)
            {
                case CardInfo.Rarity.Common:
                    if (r.Next(10) == 0)
                    {
                        ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, ModdingUtils.Utils.Cards.instance.GetCardWithName("Genie: Smiles"), false, "", 2f, 2f);
                        ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, card, false, "", 2f, 2f);
                    }
                    else
                    {
                        ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, ModdingUtils.Utils.Cards.instance.GetCardWithName("Genie: Granted"), false, "", 2f, 2f);
                    }
                    break;
                case CardInfo.Rarity.Uncommon:
                    if (r.Next(10) == 0)
                    {
                        ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, ModdingUtils.Utils.Cards.instance.GetCardWithName("Genie: Eternity"), false, "", 2f, 2f);
                        player.data.stats.GetRootData().lockedCard = card;
                    }
                    else
                    {
                        ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, ModdingUtils.Utils.Cards.instance.GetCardWithName("Genie: Fee"), false, "", 2f, 2f);
                    }
                    break;
                case CardInfo.Rarity.Rare:
                    if (r.Next(10) == 0)
                    {
                        ModdingUtils.Utils.Cards.instance.RemoveAllCardsFromPlayer(player);
                        ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, ModdingUtils.Utils.Cards.instance.GetCardWithName("Genie: Greed"), false, "", 2f, 2f);
                    }
                    else
                    {
                        ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, ModdingUtils.Utils.Cards.instance.GetCardWithName("Genie: Death"), false, "", 2f, 2f);
                    }
                    break;
            }*/

            float rarity = RarityUtils.GetRarityData(card.rarity).relativeRarity;
            float Commonrarity = RarityUtils.GetRarityData(CardInfo.Rarity.Common).relativeRarity;
            float Uncommonrarity = RarityUtils.GetRarityData(CardInfo.Rarity.Uncommon).relativeRarity;
            float Rarerarity = RarityUtils.GetRarityData(CardInfo.Rarity.Uncommon).relativeRarity;
            if(rarity >= Commonrarity)
            {
                if (r.Next(Mathf.Max(1,(int)(10 *(Commonrarity/rarity)))) == 0)
                {
                    ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, CardResgester.ModCards["Genie_Smiles"], false, "", 2f, 2f);
                    ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, card, false, "", 2f, 2f);
                }
                else
                {
                    ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, CardResgester.ModCards["Genie_Granted"], false, "", 2f, 2f);
                }
            }
            else if(rarity >= Uncommonrarity)
            {
                float percent = ((rarity - Commonrarity) / (Uncommonrarity - Commonrarity))* 100;
                float odds = (float)(r.NextDouble() * 100);
                if (odds < 10-percent)
                {
                    ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, CardResgester.ModCards["Genie_Smiles"], false, "", 2f, 2f);
                    ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, card, false, "", 2f, 2f);
                }
                else if(odds < 100- (percent*2))
                {
                    ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, CardResgester.ModCards["Genie_Granted"], false, "", 2f, 2f);
                }
                else if(odds < 390 - (percent * 3))
                {
                    ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, CardResgester.ModCards["Genie_Fee"], false, "", 2f, 2f);
                }
                else
                {
                    ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, CardResgester.ModCards["Genie_Eternity"], false, "", 2f, 2f);
                    player.data.stats.GetRootData().lockedCard = card;
                }
            }
            else if(rarity >= Rarerarity)
            {
                float percent = ((rarity - Uncommonrarity) / (Rarerarity - Uncommonrarity)) * 100;
                float odds = (float)(r.NextDouble() * 100);
                if (odds < 90 - (percent*2))
                {
                    ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, CardResgester.ModCards["Genie_Fee"], false, "", 2f, 2f);
                }
                else if (odds > (percent * 2) && odds < 100 - percent/10)
                {
                    ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, CardResgester.ModCards["Genie_Eternity"], false, "", 2f, 2f);
                    player.data.stats.GetRootData().lockedCard = card;
                }
                else if (odds < 390 - (percent * 3))
                {
                    ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, CardResgester.ModCards["Genie_Death"], false, "", 2f, 2f);
                }
                else
                {
                    ModdingUtils.Utils.Cards.instance.RemoveAllCardsFromPlayer(player);
                    ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, CardResgester.ModCards["Genie_Greed"], false, "", 2f, 2f);
                }
            }
            else
            {
                if (r.Next(Mathf.Max(1, (int)(10 * (rarity / Rarerarity)))) == 0)
                {
                    ModdingUtils.Utils.Cards.instance.RemoveAllCardsFromPlayer(player);
                    ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, CardResgester.ModCards["Genie_Greed"], false, "", 2f, 2f);
                }
                else
                {
                    ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, CardResgester.ModCards["Genie_Death"], false, "", 2f, 2f);
                }
            }
            

            ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, card, false, "", 2f, 2f);
            RootCards.instance.StartCoroutine(ShowCard(player, card));
            if (player.data.view.IsMine && !player.GetAdditionalData().bankAccount.HasFunds(Genie.wishes)) Genie.Genie_Shop.Hide();
        }
        public static IEnumerator ShowCard(Player player, CardInfo card)
        {
            yield return ModdingUtils.Utils.CardBarUtils.instance.ShowImmediate(player, card, 2f);

            yield break;
        }


        private GameObject GetCardVisuals(CardInfo card, GameObject parent)
        {
            GameObject cardObj = GameObject.Instantiate<GameObject>(card.gameObject, parent.gameObject.transform);
            cardObj.SetActive(true);
            cardObj.GetComponentInChildren<CardVisuals>().firstValueToSet = true;
            RectTransform rect = cardObj.GetOrAddComponent<RectTransform>();
            rect.localScale = 100f * Vector3.one;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.pivot = new Vector2(0.5f, 0.5f);

            GameObject back = FindObjectInChildren(cardObj, "Back");
            try
            {
                GameObject.Destroy(back);
            }
            catch { }
            FindObjectInChildren(cardObj, "BlockFront")?.SetActive(false);

            var canvasGroups = cardObj.GetComponentsInChildren<CanvasGroup>();
            foreach (var canvasGroup in canvasGroups)
            {
                canvasGroup.alpha = 1;
            }

            ItemShops.ItemShops.instance.ExecuteAfterSeconds(0.2f, () =>
            {
                //var particles = cardObj.GetComponentsInChildren<GeneralParticleSystem>().Select(system => system.gameObject);
                //foreach (var particle in particles)
                //{
                //    UnityEngine.GameObject.Destroy(particle);
                //}

                var rarities = cardObj.GetComponentsInChildren<CardRarityColor>();

                foreach (var rarity in rarities)
                {
                    try
                    {
                        rarity.Toggle(true);
                    }
                    catch
                    {

                    }
                }

                var titleText = FindObjectInChildren(cardObj, "Text_Name").GetComponent<TextMeshProUGUI>();

                if ((titleText.color.r < 0.18f) && (titleText.color.g < 0.18f) && (titleText.color.b < 0.18f))
                {
                    titleText.color = new Color32(200, 200, 200, 255);
                }
            });

            return cardObj;
        }
        private static GameObject FindObjectInChildren(GameObject gameObject, string gameObjectName)
        {
            Transform[] children = gameObject.GetComponentsInChildren<Transform>(true);
            return (from item in children where item.name == gameObjectName select item.gameObject).FirstOrDefault();
        }
    }
