// -----------------------------------------------------------------------
// <copyright file="PartnerOfferController.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Storefront.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using BusinessLogic;
    using Models;

    /// <summary>
    /// Serves partner offers to callers.
    /// </summary>
    [RoutePrefix("api/partnerOffers")]
    public class PartnerOfferController : BaseController
    {
        /// <summary>
        /// Retrieves all the active offers the partner has configured.
        /// </summary>
        /// <returns>The active partner offers.</returns>
        [Route("")]
        [HttpGet]
        public async Task<OfferCatalogViewModel> GetOffersCatalog()
        {
            var isBrandingConfigured = await  ApplicationDomain.Instance.PortalBranding.IsConfiguredAsync().ConfigureAwait(false);
            var isOffersConfigured = await ApplicationDomain.Instance.OffersRepository.IsConfiguredAsync().ConfigureAwait(false);
            var isPaymentConfigured = await ApplicationDomain.Instance.PaymentConfigurationRepository.IsConfiguredAsync().ConfigureAwait(false);

            var microsoftOffers = await ApplicationDomain.Instance.OffersRepository.RetrieveMicrosoftOffersAsync().ConfigureAwait(false);
            var partnerOffers = await ApplicationDomain.Instance.OffersRepository.RetrieveAsync().ConfigureAwait(false);


            var offerCatalogViewModel = new OfferCatalogViewModel
            {
                IsPortalConfigured = isBrandingConfigured && isOffersConfigured && isPaymentConfigured
            };

            if (offerCatalogViewModel.IsPortalConfigured)
            {
                foreach (var offer in partnerOffers)
                {
                    // TODO :: Handle Microsoft offer being pulled back due to EOL. 
                    var microsoftOfferItem = microsoftOffers.Where(msOffer => msOffer.Offer.Id == offer.MicrosoftOfferId).FirstOrDefault();

                    // temporarily remove the partner offer from catalog display if the corresponding Microsoft offer does not exist. 
                    if (microsoftOfferItem != null)
                    {
                        offer.Thumbnail = microsoftOfferItem.ThumbnailUri;
                    }                    
                    else
                    {
                        // temporary fix - remove the items from the collection by marking it as Inactive.
                        offer.IsInactive = true;
                    }
                }

                offerCatalogViewModel.Offers = partnerOffers.Where(offer => offer.IsInactive == false);
            }

            return offerCatalogViewModel;
        }
    }
}