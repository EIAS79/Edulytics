(() => {
    "use strict";

    const youtubeWatchPrefix = "https://www.youtube.com/watch?v=";
    const youtubeSearchPrefix = "https://www.youtube.com/";
    const youtubeEmbedPrefix = "https://www.youtube-nocookie.com/embed/";
    const thumbnailPrefix = "https://i.ytimg.com/";

    function compact(value) {
        const number = Number(value || 0);
        if (!Number.isFinite(number) || number <= 0) return "—";
        return new Intl.NumberFormat(undefined, {
            notation: "compact",
            maximumFractionDigits: 1
        }).format(number);
    }

    function safeUrl(value, prefix) {
        if (typeof value !== "string" || !value.startsWith(prefix)) {
            return null;
        }

        try {
            const parsed = new URL(value);
            return parsed.protocol === "https:" ? parsed.toString() : null;
        } catch {
            return null;
        }
    }

    function create(tag, className, text) {
        const node = document.createElement(tag);
        if (className) node.className = className;
        if (text !== undefined && text !== null) node.textContent = text;
        return node;
    }

    function normalizeResult(result) {
        return {
            available: Boolean(result?.available),
            gradeBand: result?.gradeBand || "",
            searchQuery: result?.searchQuery || "",
            searchUrl: result?.searchUrl || "",
            preferredChannels: Array.isArray(result?.preferredChannels)
                ? result.preferredChannels
                : [],
            featured: result?.featured || null,
            related: Array.isArray(result?.related) ? result.related : [],
            usedPreferredChannels: Boolean(result?.usedPreferredChannels),
            message: result?.message || ""
        };
    }

    function initStudio(root) {
        const endpoint = root.dataset.endpoint;
        if (!endpoint) return;

        const form = root.querySelector("[data-youtube-search-form]");
        const queryInput = root.querySelector("[data-youtube-query]");
        const playerShell = root.querySelector("[data-youtube-player-shell]");
        const title = root.querySelector("[data-youtube-feature-title]");
        const channel = root.querySelector("[data-youtube-feature-channel]");
        const featureMeta = root.querySelector("[data-youtube-feature-meta]");
        const match = root.querySelector("[data-youtube-match]");
        const matchDetail = root.querySelector("[data-youtube-match-detail]");
        const views = root.querySelector("[data-youtube-views]");
        const likes = root.querySelector("[data-youtube-likes]");
        const duration = root.querySelector("[data-youtube-duration]");
        const status = root.querySelector("[data-youtube-status]");
        const gradeBand = root.querySelector("[data-youtube-grade-band]");
        const channels = root.querySelector("[data-youtube-channels]");
        const related = root.querySelector("[data-youtube-related]");
        const resultCount = root.querySelector("[data-youtube-result-count]");
        const watch = root.querySelector("[data-youtube-watch]");
        const allResults = root.querySelector("[data-youtube-all-results]");

        let requestVersion = 0;
        const fallbackPlayer = playerShell?.querySelector("iframe") || null;
        const fallbackSnapshot = fallbackPlayer
            ? {
                src: fallbackPlayer.getAttribute("src") || "",
                title: fallbackPlayer.getAttribute("title") || ""
            }
            : null;
        const fallbackFeatureSnapshot = {
            title: title?.textContent || "",
            channel: channel?.textContent || "YouTube",
            match: match?.textContent || "—",
            matchDetail: matchDetail?.textContent || "—",
            views: views?.textContent || "—",
            likes: likes?.textContent || "—",
            duration: duration?.textContent || "—",
            watchHref: watch?.getAttribute("href") || "#",
            metaNodes: featureMeta
                ? Array.from(featureMeta.childNodes).map(node => node.cloneNode(true))
                : []
        };

        function setBusy(isBusy) {
            root.classList.toggle("is-loading", isBusy);
            form?.querySelector("button")?.toggleAttribute("disabled", isBusy);
            if (queryInput) queryInput.setAttribute("aria-busy", isBusy ? "true" : "false");
        }

        function restoreFallbackFeature(emptyMessage) {
            if (playerShell) {
                playerShell.replaceChildren();

                if (fallbackSnapshot) {
                    const iframe = create("iframe");
                    iframe.dataset.youtubePlayer = "";
                    iframe.src = fallbackSnapshot.src;
                    iframe.title = fallbackSnapshot.title || "YouTube lesson video";
                    iframe.loading = "lazy";
                    iframe.referrerPolicy = "strict-origin-when-cross-origin";
                    iframe.allow =
                        "accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share";
                    iframe.allowFullscreen = true;
                    playerShell.appendChild(iframe);
                } else {
                    playerShell.appendChild(
                        create(
                            "div",
                            "yt-studio-player__empty",
                            emptyMessage ||
                                "No embeddable video passed the current lesson-match checks."
                        )
                    );
                }
            }

            if (title) title.textContent = fallbackFeatureSnapshot.title;
            if (channel) channel.textContent = fallbackFeatureSnapshot.channel;
            if (match) match.textContent = fallbackFeatureSnapshot.match;
            if (matchDetail) matchDetail.textContent = fallbackFeatureSnapshot.matchDetail;
            if (views) views.textContent = fallbackFeatureSnapshot.views;
            if (likes) likes.textContent = fallbackFeatureSnapshot.likes;
            if (duration) duration.textContent = fallbackFeatureSnapshot.duration;

            if (featureMeta) {
                featureMeta.replaceChildren(
                    ...fallbackFeatureSnapshot.metaNodes.map(node => node.cloneNode(true))
                );
            }

            if (watch) {
                watch.href = fallbackFeatureSnapshot.watchHref;
                if (fallbackSnapshot && fallbackFeatureSnapshot.watchHref !== "#") {
                    watch.removeAttribute("aria-disabled");
                } else {
                    watch.setAttribute("aria-disabled", "true");
                }
            }
        }

        function renderPlayer(video, autoplay = false) {
            if (!playerShell || !video) return;

            const embed = safeUrl(video.embedUrl, youtubeEmbedPrefix);
            if (!embed) return;

            playerShell.replaceChildren();
            const iframe = create("iframe");
            iframe.dataset.youtubePlayer = "";
            iframe.src = autoplay
                ? embed + (embed.includes("?") ? "&" : "?") + "autoplay=1"
                : embed;
            iframe.title = video.title || "YouTube lesson video";
            iframe.loading = "lazy";
            iframe.referrerPolicy = "strict-origin-when-cross-origin";
            iframe.allow =
                "accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share";
            iframe.allowFullscreen = true;
            playerShell.appendChild(iframe);
        }

        function renderFeatured(video, autoplay = false) {
            if (!video) return;

            renderPlayer(video, autoplay);

            if (title) title.textContent = video.title || "YouTube lesson video";
            if (channel) channel.textContent = video.channelTitle || "YouTube";

            const percent = Number(video.matchPercent || video.relevancePercent || 0);
            const matchText = percent > 0 ? percent + "%" : "—";
            if (match) match.textContent = matchText;
            if (matchDetail) matchDetail.textContent = matchText;
            if (views) views.textContent = compact(video.viewCount);
            if (likes) likes.textContent = compact(video.likeCount);
            if (duration) duration.textContent = video.durationLabel || "—";

            if (featureMeta) {
                featureMeta.replaceChildren();
                if (video.isPreferredChannel) {
                    featureMeta.appendChild(
                        create("span", "yt-studio-meta-pill yt-studio-meta-pill--preferred", "Preferred channel bonus")
                    );
                }
                if (video.durationLabel) {
                    featureMeta.appendChild(
                        create("span", "yt-studio-meta-pill", video.durationLabel)
                    );
                }
            }

            const watchUrl = safeUrl(video.watchUrl, youtubeWatchPrefix);
            if (watch && watchUrl) {
                watch.href = watchUrl;
                watch.removeAttribute("aria-disabled");
            }
        }

        function renderChannels(items) {
            if (!channels) return;

            channels.replaceChildren();
            if (!items.length) {
                channels.appendChild(
                    create("span", "yt-studio-chip yt-studio-chip--muted", "Preferred channels unavailable")
                );
                return;
            }

            items.forEach((item, index) => {
                const href = safeUrl(item.channelSearchUrl, youtubeSearchPrefix);
                if (!href) return;

                const link = create("a", "yt-studio-chip");
                link.href = href;
                link.target = "_blank";
                link.rel = "noopener noreferrer";
                link.title = "Search this lesson inside " + (item.name || "YouTube");

                const indexNode = create(
                    "span",
                    "yt-studio-chip__index",
                    String(index + 1).padStart(2, "0")
                );
                const nameNode = create("span", "", item.name || item.handle || "YouTube");
                link.append(indexNode, nameNode);
                channels.appendChild(link);
            });
        }

        function renderRelated(items) {
            if (!related) return;
            related.replaceChildren();

            if (resultCount) resultCount.textContent = String(items.length);

            if (!items.length) {
                const empty = create(
                    "div",
                    "yt-studio-related-empty",
                    "No additional lesson-matched videos were returned."
                );
                related.appendChild(empty);
                return;
            }

            items.forEach(video => {
                const card = create("article", "yt-studio-result-card");
                const media = create("button", "yt-studio-result-card__media");
                media.type = "button";
                media.title = "Play " + (video.title || "video");

                const thumb = safeUrl(video.thumbnailUrl, thumbnailPrefix);
                if (thumb) {
                    const image = create("img");
                    image.src = thumb;
                    image.alt = "";
                    image.loading = "lazy";
                    image.referrerPolicy = "no-referrer";
                    media.appendChild(image);
                } else {
                    media.appendChild(create("span", "yt-studio-result-card__thumb-fallback", "▶"));
                }

                const durationBadge = create(
                    "span",
                    "yt-studio-result-card__duration",
                    video.durationLabel || ""
                );
                if (video.durationLabel) media.appendChild(durationBadge);

                const body = create("div", "yt-studio-result-card__body");
                body.appendChild(
                    create("p", "yt-studio-result-card__channel", video.channelTitle || "YouTube")
                );
                body.appendChild(
                    create("h4", "yt-studio-result-card__title", video.title || "YouTube lesson video")
                );

                const signals = create("div", "yt-studio-result-card__signals");
                signals.appendChild(
                    create(
                        "span",
                        video.isPreferredChannel
                            ? "yt-studio-result-signal is-preferred"
                            : "yt-studio-result-signal",
                        (Number(video.matchPercent || video.relevancePercent || 0) || 0) + "% match"
                    )
                );
                signals.appendChild(
                    create("span", "yt-studio-result-signal", compact(video.viewCount) + " views")
                );
                body.appendChild(signals);

                const actions = create("div", "yt-studio-result-card__actions");
                const play = create("button", "yt-studio-result-play", "Play here");
                play.type = "button";
                play.addEventListener("click", () => {
                    renderFeatured(video, true);
                    root.scrollIntoView({ behavior: "smooth", block: "start" });
                });

                const href = safeUrl(video.watchUrl, youtubeWatchPrefix);
                const open = create("a", "yt-studio-result-open", "YouTube ↗");
                if (href) {
                    open.href = href;
                    open.target = "_blank";
                    open.rel = "noopener noreferrer";
                } else {
                    open.href = "#";
                    open.setAttribute("aria-disabled", "true");
                }

                actions.append(play, open);
                body.appendChild(actions);

                media.addEventListener("click", () => renderFeatured(video, true));
                card.append(media, body);
                related.appendChild(card);
            });
        }

        function renderUnavailable(result) {
            if (gradeBand && result.gradeBand) gradeBand.textContent = result.gradeBand;
            renderChannels(result.preferredChannels);

            const searchUrl = safeUrl(result.searchUrl, youtubeSearchPrefix);
            if (allResults && searchUrl) allResults.href = searchUrl;
            if (status) {
                status.textContent =
                    result.message ||
                    "YouTube search is temporarily unavailable. Use the lesson-scoped channel links instead.";
            }

            restoreFallbackFeature(
                "Use the YouTube lesson search or a preferred channel below."
            );
        }

        function renderResult(raw) {
            const result = normalizeResult(raw);

            if (gradeBand && result.gradeBand) gradeBand.textContent = result.gradeBand;
            if (queryInput && result.searchQuery && !queryInput.value) {
                queryInput.placeholder = result.searchQuery;
            }

            renderChannels(result.preferredChannels);

            const searchUrl = safeUrl(result.searchUrl, youtubeSearchPrefix);
            if (allResults && searchUrl) allResults.href = searchUrl;

            if (status) {
                status.textContent =
                    result.message ||
                    (result.usedPreferredChannels
                        ? "Selected from the preferred teaching channels for this grade band."
                        : "Selected from the strongest YouTube match for this lesson.");
            }

            if (!result.available) {
                renderUnavailable(result);
                renderRelated([]);
                return;
            }

            if (result.featured) {
                renderFeatured(result.featured);
            } else {
                restoreFallbackFeature(
                    "No embeddable video passed the current lesson-match checks."
                );
            }

            renderRelated(result.related);
        }

        async function load(query) {
            const version = ++requestVersion;
            setBusy(true);

            if (status) {
                status.textContent = query
                    ? "Searching YouTube within this lesson topic…"
                    : "Finding the strongest lesson-matched video…";
            }

            try {
                const url = new URL(endpoint, window.location.origin);
                if (query) url.searchParams.set("q", query);

                const response = await fetch(url, {
                    method: "GET",
                    credentials: "same-origin",
                    headers: { Accept: "application/json" }
                });

                if (!response.ok) {
                    throw new Error("YouTube discovery request failed with " + response.status);
                }

                const data = await response.json();
                if (version !== requestVersion) return;
                renderResult(data);
            } catch {
                if (version !== requestVersion) return;

                restoreFallbackFeature(
                    "YouTube discovery could not load right now. Use the lesson-scoped YouTube links instead."
                );

                if (status) {
                    status.textContent =
                        "YouTube discovery could not load right now. The lesson remains available and you can use the direct YouTube links.";
                }
                if (resultCount) resultCount.textContent = "0";
                if (related) {
                    related.replaceChildren(
                        create(
                            "div",
                            "yt-studio-related-empty",
                            "Search results are temporarily unavailable."
                        )
                    );
                }
            } finally {
                if (version === requestVersion) setBusy(false);
            }
        }

        watch?.addEventListener("click", event => {
            if (watch.getAttribute("aria-disabled") === "true") {
                event.preventDefault();
            }
        });

        form?.addEventListener("submit", event => {
            event.preventDefault();
            load((queryInput?.value || "").trim());
        });

        // Resolve the lesson video automatically when the lesson opens.
        // Server-side caching prevents repeated YouTube API work for the same
        // lesson/search context, while the manual search remains available for
        // learner refinement.
        load("");
    }

    document
        .querySelectorAll("[data-youtube-studio]")
        .forEach(initStudio);
})();
